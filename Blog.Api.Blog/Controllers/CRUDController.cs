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
            var blogViewModel = new BlogViewModel()
            {
                Id = blog.Id, 
                Title = blog.Title,
                Description = blog.Description,
                Tags = blog.Tags,
                Category = blog.Category,
                MainBlogText = blog.MainBlogText,
                Image = blog.Image,
                CreationDate = blog.CreationDate.ToString("d")
            };
            return Ok(blogViewModel);
        }

        [HttpGet]
        [EnableCors(CorsOrigins.FrontPolicy)]
        public async Task<IActionResult> GetAll()
        {
            var blogs = await _blogProvider.GetAllBlogs();
            
            var blogViewModel = 
                blogs.Select(blog=> new BlogViewModel() 
                {
                    Id = blog.Id, 
                    Title = blog.Title,
                    Description = blog.Description,
                    Tags = blog.Tags,
                    Category = blog.Category,
                    MainBlogText = blog.MainBlogText,
                    Image = blog.Image,
                    CreationDate = blog.CreationDate.ToString("d")
                }).ToList();
            
            return Ok(blogViewModel);
        }
        
        [HttpPost]
        [Route("/api/blog/search")]
        [EnableCors(CorsOrigins.FrontPolicy)]
        public async Task<IActionResult> Search(BlogFindDto title)
        {
            var blogs = await _blogProvider.GetAllBlogsByTitle(title);
            
            var blogViewModel = 
                blogs.Select(blog=> new BlogViewModel() 
                {
                    Id = blog.Id, 
                    Title = blog.Title,
                    Description = blog.Description,
                    Tags = blog.Tags,
                    Category = blog.Category,
                    MainBlogText = blog.MainBlogText,
                    Image = blog.Image,
                    CreationDate = blog.CreationDate.ToString("d")
                }).ToList();
            
            return Ok(blogViewModel);
        }
        
        [HttpPost]
        [Route("/api/blog/tags")]
        [EnableCors(CorsOrigins.FrontPolicy)]
        public async Task<IActionResult> GetAllByTag(BlogFindDto tag)
        {
            var blogs = await _blogProvider.GetAllBlogsByTag(tag);
            
            var blogViewModel = 
                blogs.Select(blog=> new BlogViewModel() 
                {
                    Id = blog.Id, 
                    Title = blog.Title,
                    Description = blog.Description,
                    Tags = blog.Tags,
                    Category = blog.Category,
                    MainBlogText = blog.MainBlogText,
                    Image = blog.Image,
                    CreationDate = blog.CreationDate.ToString("d")
                }).ToList();
            
            return Ok(blogViewModel);
        }
        
        [HttpPost]
        [Route("/api/blog/category")]
        [EnableCors(CorsOrigins.FrontPolicy)]
        public async Task<IActionResult> GetAllByCategory(BlogFindDto category)
        {
            var blogs = await _blogProvider.GetAllBlogsByCategory(category);
            
            var blogViewModel = 
                blogs.Select(blog=> new BlogViewModel() 
                {
                    Id = blog.Id, 
                    Title = blog.Title,
                    Description = blog.Description,
                    Tags = blog.Tags,
                    Category = blog.Category,
                    MainBlogText = blog.MainBlogText,
                    Image = blog.Image,
                    CreationDate = blog.CreationDate.ToString("d")
                }).ToList();
            
            return Ok(blogViewModel);
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
