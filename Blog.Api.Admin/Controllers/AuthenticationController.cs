using BazarJok.Contracts.ViewModels;
using BazarJok.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;
using BazarJok.Contracts.Options;
using Microsoft.AspNetCore.Cors;

namespace BazarJok.Api.Admin.Controllers
{
    [Route("api/auth/[action]")]
    [EnableCors(CorsOrigins.AdminPanelPolicy)]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly AdminAuthenticationService _authenticationService;

        public AuthenticationController(AdminAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(LoginAdminViewModel loginUserViewModel)
        {
            try
            {
                var token = await _authenticationService
                    .Authenticate(loginUserViewModel.Login, loginUserViewModel.Password);
                
                return token != null ? (IActionResult)Ok(token) : Unauthorized();
            }
            catch (ArgumentException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> GetUserData()
        {
            var admin = await _authenticationService
                .GetAdminByHeaders(Request.Headers[HeaderNames.Authorization].ToArray());
            
            return Ok(new { 
                admin.Id,
                admin.Login,
                admin.Role
            });
        }

    }

}
