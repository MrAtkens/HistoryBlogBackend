﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BazarJok.Contracts.Dtos;
using BazarJok.Contracts.Options;
using BazarJok.DataAccess.Models;
using BazarJok.DataAccess.Providers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BazarJok.Services.Identity
{
    public class UserAuthenticationService<TUser, TProvider> 
        where TUser : User 
        where TProvider : AccountProvider<TUser>
    {
        private readonly TProvider _provider;

        private SecretOption SecretOptions { get; }

        public UserAuthenticationService(TProvider provider, IOptions<SecretOption> secretOptions)
        {
            SecretOptions = secretOptions.Value;
            this._provider = provider;
        }

        /// <summary>
        ///     Getting a token before creating a new user
        /// </summary>
        /// <param name="newUser">Data transfer object for registration new user</param>
        /// <exception cref="ArgumentException">User is already exists</exception>
        /// <returns></returns>
        public async Task<string> Register(UserCreationDto newUser)
        {
            // Try to get a data from the newUser parameter
            var user = await _provider.FirstOrDefault(x => x.Email == newUser.Email);

            if (user is not null)
                throw new ArgumentException("This user is already exists");
            
            
            // Add new user to table
            await _provider.Add(new UserCreationDto
            {
                Email = newUser.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password)
            });
            
            return GenerateJwtToken(newUser.Email);
        }

        

        /// <summary>
        ///     Get Jwt token by exited user
        /// </summary>
        /// <param name="emailOrPhone"></param>
        /// <param name="password"></param>
        /// <exception cref="ArgumentException">User is not found</exception>
        /// <returns>Jwt token</returns>
        public async Task<string> Authenticate(string emailOrPhone, string password)
        {
            // Find data by arguments
            var user = await _provider.GetByEmail(emailOrPhone);

            // if user is not found, throw exception
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new ArgumentException("Incorrect password");

            return GenerateJwtToken(user.Email);
        }

        


        private string GenerateJwtToken(string phone)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecretOptions.JwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.MobilePhone, phone),
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
        private UserClaimsDto DecryptToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            if (tokenS?.Claims is List<Claim> claims)
            {
                return new UserClaimsDto()
                {
                    Email = claims[0].Value
                };
            }

            throw new ArgumentException();
        }

        /// <summary>
        /// Gets User by headers from Request
        /// Usage in controllers: 
        /// GetUserByHeaders(Request.Headers[HeaderNames.Authorization].ToArray())
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async Task<TUser> GetUserByHeaders(string[] headers)
        {
            var token = headers[0].Replace("Bearer ", "");

            return await _provider.GetByEmail(DecryptToken(token).Email);
        }
    }
}
