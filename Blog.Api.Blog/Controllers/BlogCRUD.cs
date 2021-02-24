using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly AdminProvider _adminProvider;
        private readonly BlogService _blogService;

        public BlogController(BlogProvider blogProvider, AdminProvider adminProvider, BlogService blogService)
        {
            _blogProvider = blogProvider;
            _adminProvider = adminProvider;
            _blogService = blogService;
        }

        [HttpGet("{id}")]
        [EnableCors(CorsOrigins.FrontPolicy)]
        public async Task<IActionResult> Get(Guid id)
        {
            var blog = await _blogService.GetById(id);
            if (blog is null)
                return NotFound("Support is not found");
            await _blogService.IncreaseView(blog);
            var blogViewModel = new BlogViewModel()
            {
                Id = blog.Id, 
                Title = blog.Title,
                Description = blog.Description,
                AuthorName = blog.Author.Login,
                Tags = blog.Tags,
                Category = blog.Category,
                MainBlogText = blog.MainBlogText,
                Image = blog.Image,
                CreationDate = blog.CreationDate.ToString("dddd dd MMMM yyyy", 
                    CultureInfo.CreateSpecificCulture("ru-RU"))
            };
            return Ok(blogViewModel);
        }
        
        [HttpGet]
        [Route("/api/blog/featured/{id?}")]
        [EnableCors(CorsOrigins.FrontPolicy)]
        public async Task<IActionResult> SetFeatured(Guid id)
        {
            var blog = await _blogService.GetById(id);
            if (blog is null)
                return NotFound("Support is not found");
            await _blogService.SetIsFeatured(blog);
            return Ok();
        }

        [HttpGet]
        [EnableCors(CorsOrigins.FrontPolicy)]
        public async Task<IActionResult> GetAll()
        {
            var blogs = await _blogProvider.GetAllBlogs();
            return Ok(BlogGetHelper.getBlogsViewModel(blogs));
        }
        
        
        [HttpGet]
        [Route("/api/blog/search")]
        [EnableCors(CorsOrigins.FrontPolicy)]
        public async Task<IActionResult> Search(BlogFindDto title)
        {
            var blogs = await _blogProvider.GetAllBlogsByTitle(title);
            return Ok(BlogGetHelper.getBlogsViewModel(blogs));
        }
        
        [HttpGet]
        [Route("/api/blog/tags/{line?}")]
        [EnableCors(CorsOrigins.FrontPolicy)]
        public async Task<IActionResult> GetAllByTag(string? line)
        {
            var blogs = await _blogProvider.GetAllBlogsByTag(line);
            return Ok(BlogGetHelper.getBlogsViewModel(blogs));
        }
        
        [HttpGet]
        [Route("/api/blog/category/{line?}")]
        [EnableCors(CorsOrigins.FrontPolicy)]
        public async Task<IActionResult> GetAllByCategory(string? line)
        {
            var blogs = await _blogProvider.GetAllBlogsByCategory(line);
            return Ok(BlogGetHelper.getBlogsViewModel(blogs));
        }
        
        [HttpGet]
        [Route("/api/blog/featured")]
        [EnableCors(CorsOrigins.FrontPolicy)]
        public async Task<IActionResult> GetFeaturedBlogs()
        {
            var blogs = await _blogProvider.GetFeaturedBlogs();
            return Ok(BlogGetHelper.getBlogsViewModel(blogs));
        }
        
        [HttpPost]
        [Route("/api/blog/related")]
        [EnableCors(CorsOrigins.FrontPolicy)]
        public async Task<IActionResult> GetRelated(RelatedBlogsDto relatedBlogs)
        {
            var blogs = await _blogProvider.GetAllBlogsByCategory(relatedBlogs.Category);

            int index = blogs.FindIndex(blog => blog.Id == relatedBlogs.Id);
            List<BazarJok.DataAccess.Models.Blog> relatedBlog = new List<BazarJok.DataAccess.Models.Blog>();
            if (blogs.Count > 3)
            {
                if (blogs.Count == index + 1)
                {
                    relatedBlog.Add(blogs[index - 1]);
                    relatedBlog.Add(blogs[index - 2]);
                    relatedBlog.Add(blogs[index - 3]);
                }
                else if (blogs.Count == index + 2)
                {
                    relatedBlog.Add(blogs[index - 1]);
                    relatedBlog.Add(blogs[index - 2]);
                    relatedBlog.Add(blogs[index + 1]);
                }
                else if (index == 0) {
                    relatedBlog.Add(blogs[index + 3]);
                    relatedBlog.Add(blogs[index + 2]);
                    relatedBlog.Add(blogs[index + 1]);
                }
                else
                {
                    relatedBlog.Add(blogs[index + 2]);
                    relatedBlog.Add(blogs[index + 1]);
                    relatedBlog.Add(blogs[index - 1]);
                }
            }

            return Ok(BlogGetHelper.getBlogsViewModel(blogs));
        }
        
        [HttpGet]
        [Route("/api/blog/admin")]
        [EnableCors(CorsOrigins.AdminPanelPolicy)]
        public async Task<IActionResult> GetAdminAll()
        {
            var blogs = await _blogProvider.GetAllBlogs();
            return Ok(BlogGetHelper.getAdminBlogsViewModel(blogs));
        }
        
        [HttpGet]
        [Route("/api/blog/author/{id?}")]
        [EnableCors(CorsOrigins.AdminPanelPolicy)]
        public async Task<IActionResult> GetAuthorBlog(Guid id)
        {
            Admin author = await _adminProvider.GetById(id);
            var blogs = await _blogProvider.GetBlogsByAuthor(author);
            return Ok(BlogGetHelper.getAdminBlogsViewModel(blogs));
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
            return NoContent();
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
            return NoContent();
        }

    }

}
