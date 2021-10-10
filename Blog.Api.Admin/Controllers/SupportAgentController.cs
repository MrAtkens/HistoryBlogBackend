using System;
using System.Linq;
using System.Threading.Tasks;
using GeekBlog.Contracts.Attributes;
using GeekBlog.Contracts.Options;
using GeekBlog.Contracts.ViewModels;
using GeekBlog.DataAccess.Models.Enums;
using GeekBlog.DataAccess.Providers;
using GeekBlog.DataAccess.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace GeekBlog.Api.Admin.Controllers
{
    [Route("api/supportAgent/")]
    [AdminAuthorized(roles:AdminRole.Admin)]
    [EnableCors(CorsOrigins.AdminPanelPolicy)]
    [ApiController]
    public class SupportAgentController : ControllerBase
    {
        private readonly EntityAdminProvider _adminProvider;

        public SupportAgentController(EntityAdminProvider adminProvider)
        {
            _adminProvider = adminProvider;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _adminProvider.GetById(id);
            if (user is null)
                return NotFound("Support is not found");

            var supportViewModel = new AdminViewModel { Id = id, Login = user.Login, CreationDate = user.CreationDate.ToString("d") };

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
                    CreationDate = support.CreationDate.ToString("d")
                }).ToList();
            
            return Ok(supportViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> Post(SupportViewModel support)
        {
            DataAccess.Models.Admin admin = new DataAccess.Models.Admin { Login = support.Login,
                PasswordHash = support.Password };
            await _adminProvider.Add(admin);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, SupportViewModel supportViewModel)
        {
            var support = await _adminProvider.GetById(id);
            if (support is null)
                return NotFound("Support is not found");
            
            support.Login = supportViewModel.Login;
            if(supportViewModel.Password != null)
                support.PasswordHash = BCrypt.Net.BCrypt.HashPassword(supportViewModel.Password);
            await _adminProvider.Edit(support);
            return Ok();
        }
        
        [HttpDelete("{id}")]
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
