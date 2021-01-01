using System;
using System.Linq;
using System.Threading.Tasks;
using BazarJok.Contracts.Attributes;
using BazarJok.Contracts.Options;
using BazarJok.Contracts.ViewModels;
using BazarJok.DataAccess.Models;
using BazarJok.DataAccess.Providers;
using BazarJok.Services.Identity;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BazarJok.Api.Admin.Controllers
{
    [Route("api/supportAgent/")]
    [AdminAuthorized(roles:AdminRole.Admin)]
    [EnableCors(CorsOrigins.AdminPanelPolicy)]
    [ApiController]
    public class SupportAgentController : ControllerBase
    {
        private readonly AdminProvider _adminProvider;
        private readonly AdminAuthenticationService _authenticationService;

        public SupportAgentController(AdminProvider adminProvider, AdminAuthenticationService authenticationService)
        {
            _adminProvider = adminProvider;
            _authenticationService = authenticationService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _adminProvider.GetById(id);
            if (user is null)
                return NotFound("Support is not found");

            var supportViewModel = new AdminViewModel { Id = id, Login = user.Login, CreationDate = user.CreationDate };

            return Ok(supportViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var supports = await _adminProvider.Get(x=>x.Role == AdminRole.Editor); 
            
            var supportViewModels = 
                supports.Select(support=> new AdminViewModel 
                {
                    Id = support.Id, 
                    Login = support.Login, 
                    CreationDate = support.CreationDate
                }).ToList();
            
            return Ok(supportViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> Post(SupportViewModel support)
        {
            await _authenticationService.AddEditor(support.Login, support.Password);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, SupportViewModel supportViewModel)
        {
            var support = await _adminProvider.GetById(id);
            if (support is null)
                return NotFound("Support is not found");
            
            support.Login = supportViewModel.Login;
            support.PasswordHash = BCrypt.Net.BCrypt.HashPassword(supportViewModel.Password);
            await _adminProvider.Edit(support);
            return Ok();
        }
        
        [HttpDelete("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var support = await _adminProvider.GetById(id);
            if (support is null)
                return NotFound("Support is not found");
            
            await _adminProvider.Remove(support);
            return Ok();
        }

    }

}
