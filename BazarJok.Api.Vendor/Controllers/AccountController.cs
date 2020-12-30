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
    [Route("api/account/")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly VendorProvider _vendorProvider;
        private readonly UserAuthenticationService<DataAccess.Models.Vendor, VendorProvider> _authenticationService;

        public AccountController(VendorProvider vendorProvider, UserAuthenticationService<DataAccess.Models.Vendor, VendorProvider> authenticationService)
        {
            _vendorProvider = vendorProvider;
            _authenticationService = authenticationService;
        }

        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var vendor = await _authenticationService
                .GetUserByHeaders(Request.Headers[HeaderNames.Authorization].ToArray());

            var vendorViewModel = new UserViewModel {Id = vendor.Id, FirstName = vendor.FirstName, LastName = vendor.LastName, Email = vendor.Email, PhoneNumber = vendor.PhoneNumber };

            return Ok(vendorViewModel);
        }

        

        [HttpPut]
        public async Task<IActionResult> Put(PutUserViewModel userViewModel)
        {
            var vendor = await _authenticationService
                .GetUserByHeaders(Request.Headers[HeaderNames.Authorization].ToArray());

            vendor.FirstName = userViewModel.FirstName;
            vendor.LastName = userViewModel.LastName;
            vendor.Email = userViewModel.Email;
            vendor.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userViewModel.Password);
            await _vendorProvider.Edit(vendor);
            return Ok();
        }


    }
}
