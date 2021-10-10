using GeekBlog.Contracts.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GeekBlog.DataAccess.Models;
using GeekBlog.Contracts.Dtos;
using GeekBlog.DataAccess.Models.Enums;
using BazarJok.DataAccess.Providers.Abstract;

namespace GeekBlog.Services.Identity
{
    public class AdminAuthenticationService
    {
        private readonly IAdminProvider _adminProvider;

        private SecretOption SecretOptions { get; }

        public AdminAuthenticationService(IAdminProvider adminProvider, IOptions<SecretOption> secretOptions)
        {
            SecretOptions = secretOptions.Value;
            this._adminProvider = adminProvider;
        }


        /// <summary>
        ///     Get Jwt token by exited user
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <exception cref="ArgumentException">User is not found</exception>
        /// <returns>Jwt token</returns>
        public async Task<string> Authenticate(string login, string password)
        {
            // Find data by arguments
            var admin = await _adminProvider.GetByLogin(login);

            // if user is not found, throw exception
            if (!BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash))
                throw new ArgumentException("Incorrect password");

            return GenerateJwtToken(admin.Login, admin.Role);
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


        /// <summary>
        ///    Token decryption
        /// </summary>
        /// <param name="token"></param>
        /// <exception cref="ArgumentException">throws when could not parse claims</exception>
        /// <returns>Owner's data</returns>
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
                        ? role
                        : throw new ArgumentException()) ?? throw new ArgumentException())
                };
            }

            throw new ArgumentException();
        }

        /// <summary>
        /// Gets Admin by headers from Request
        /// Usage in controllers: 
        /// GetAdminByHeaders(Request.Headers[HeaderNames.Authorization].ToArray())
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async Task<Admin> GetAdminByHeaders(string[] headers)
        {
            var token = headers[0].Replace("Bearer ", "");

            var result = await _adminProvider.GetByLogin(DecryptToken(token).Login);

            return result;
        }

    }

}
