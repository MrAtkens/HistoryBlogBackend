using BazarJok.Contracts.Dtos;
using BazarJok.Contracts.ViewModels;
using BazarJok.DataAccess.Providers;
using BazarJok.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BazarJok.Api.Vendor.Controllers
{
    [Route("api/auth/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly UserAuthenticationService<DataAccess.Models.Vendor, VendorProvider> _authenticationService;

        public AuthenticationController(UserAuthenticationService<DataAccess.Models.Vendor, VendorProvider> authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginUserViewModel loginUserViewModel)
        {
            try
            {
                var token = await _authenticationService
                    .Authenticate(loginUserViewModel.PhoneNumber, loginUserViewModel.Password);

                return token != null ? (IActionResult)Ok(token) : Unauthorized();
            }
            catch (ArgumentException)
            {
                return Unauthorized();
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterUserViewModel registerUser)
        {
            string token;
            try
            {
                token = await _authenticationService.Register(new UserRegistrationDto()
                {
                    FirstName = registerUser.FirstName,
                    LastName = registerUser.LastName,
                    PhoneNumber = registerUser.PhoneNumber,
                    Password = registerUser.Password
                });

            }
            catch (ArgumentException e)
            {
                return Unauthorized(e.Message);
            }
            return Ok(token);
        }


    }
}
