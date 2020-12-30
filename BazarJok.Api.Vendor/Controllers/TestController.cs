using BazarJok.DataAccess.Models;
using BazarJok.DataAccess.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BazarJok.Api.Vendor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly AdminProvider _adminProvider;

        public TestController(AdminProvider adminProvider)
        {
            this._adminProvider = adminProvider;
        }

        [HttpPost]
        public async Task<IActionResult> TestCreateUser()
        {
            var guid = Guid.NewGuid();
            await _adminProvider.Add(new Admin
            {
                Login = "Test" + guid.ToString(),
                PasswordHash = guid.ToString(),
                Role = AdminRole.Support
            });

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> TestGetAllUsers()
        {
            return Ok(await _adminProvider.GetAll());
        }


    }
}
