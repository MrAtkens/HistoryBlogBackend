using BazarJok.Contracts.Options;
using BazarJok.DataAccess.Providers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BazarJok.DataAccess.Models;
using BazarJok.Contracts.Dtos;
using Microsoft.Extensions.Caching.Memory;

namespace BazarJok.Services.Identity
{
    public class AdminAuthenticationService
    {
        private readonly AdminProvider _adminProvider;
        private readonly IMemoryCache _cache;

        private SecretOption SecretOptions { get; }

        public AdminAuthenticationService(AdminProvider adminProvider, IMemoryCache cache, IOptions<SecretOption> secretOptions)
        {
            SecretOptions = secretOptions.Value;
            _cache = cache;
            _adminProvider = adminProvider;
        }


        
        public async Task<string> Authenticate(string login, string password)
        {
            // Find data by arguments
            var admin = await _adminProvider.GetByLogin(login);

            // if user is not found, throw exception
            if (!BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash))
                throw new ArgumentException("Incorrect password");

            return GenerateJwtToken(admin.Login, admin.Role);
        }

        public async Task AddEditor(string login, string password)
        {
            var support = new Admin
            {
                Login = login,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = AdminRole.Editor
            };

            await _adminProvider.Add(support);
        }


        private string GenerateJwtToken(string login, AdminRole role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecretOptions.JwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.UserData, login),
                    new Claim(ClaimTypes.Role, role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }
        
        private AdminClaimsDto DecryptToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            if (tokenS?.Claims is List<Claim> claims)
            {
                return new AdminClaimsDto
                {
                    Login = claims[0].Value,
                    Role = (AdminRole)((Enum.TryParse(typeof(AdminRole), claims[1].Value, true, out var role)
                        ? role : throw new ArgumentException()) ?? throw new ArgumentException())
                };
            }

            throw new ArgumentException();
        }

        public async Task<Admin> GetAdminByHeaders(string[] headers)
        {
            var token = headers[0].Replace("Bearer ", "");

            return await _adminProvider.GetByLogin(DecryptToken(token).Login);
        }

    }
}
