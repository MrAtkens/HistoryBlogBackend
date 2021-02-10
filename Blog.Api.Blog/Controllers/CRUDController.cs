using System;
using System.Linq;
using System.Threading.Tasks;
using BazarJok.Contracts.Attributes;
using BazarJok.Contracts.Dtos;
using BazarJok.Contracts.Options;
using BazarJok.Contracts.ViewModels;
using BazarJok.DataAccess.Models;
using BazarJok.DataAccess.Providers;
using BazarJok.Services.Business;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Blog.Controllers
{
    [Route("api/blog/")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly BlogProvider _blogProvider;
        private readonly BlogService _blogService;

        public BlogController(BlogProvider blogProvider, BlogService blogService)
        {
            _blogProvider = blogProvider;
            _blogService = blogService;
        }

        [HttpGet("{id}")]
        [EnableCors(CorsOrigins.FrontPolicy)]
        public async Task<IActionResult> Get(Guid id)
        {
            var blog = await _blogProvider.GetById(id);
            if (blog is null)
                return NotFound("Support is not found");
            return Ok(blog);
        }

        [HttpGet]
        [EnableCors(CorsOrigins.FrontPolicy)]
        public async Task<IActionResult> GetAll()
        {
            var blogs = await _blogProvider.GetAllBlogs();
            
            var supportViewModels = 
                blogs.Select(support=> new BlogViewModel() 
                {
                    Id = support.Id, 
                    Title = support.Title,
                    Description = support.Description,
                    Tags = support.Tags,
                    Category = support.Category,
                    MainBlogText = support.MainBlogText,
                    Image = support.Image,
                    CreationDate = support.CreationDate.ToString("d")
                }).ToList();
            
            return Ok(supportViewModels);
            return Ok(blogs);
        }

        [HttpPost]
        [EnableCors(CorsOrigins.AdminPanelPolicy)]
        [AdminAuthorized(roles:AdminRole.Editor)]
        public async Task<IActionResult> Post(BlogCreationDto blogCreationDto)
        {
            await _blogService.AddBlog(blogCreationDto);
            return Ok();
        }

        [HttpPut("{id}")]
        [EnableCors(CorsOrigins.AdminPanelPolicy)]
        [AdminAuthorized(roles:AdminRole.Editor)]
        public async Task<IActionResult> Update(Guid id, BlogCreationDto blogCreationDto)
        {
            var blog = await _blogProvider.GetById(id);
            if (blog is null)
                return NotFound("Blog is not found");
            await _blogService.EditBlog(blogCreationDto, blog); // blog = Blog class
            return Ok();
        }
        
        [HttpDelete("{id}")]
        [EnableCors(CorsOrigins.AdminPanelPolicy)]
        [AdminAuthorized(roles:AdminRole.Editor)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var blog = await _blogProvider.GetById(id);
            if (blog is null)
                return NotFound("Support is not found");
            await _blogService.DeleteBlog(blog);
            return Ok();
        }

    }

}
