using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BazarJok.Contracts.Dtos;
using BazarJok.Contracts.ViewModels;
using BazarJok.DataAccess.Models;
using BazarJok.DataAccess.Providers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;

namespace BazarJok.Services.Business
{
    public class BlogService
    {
        private readonly IHostEnvironment _environment;
        private readonly BlogProvider _blogProvider;
        private readonly CategoryProvider _categoryProvider;
        private readonly AdminProvider _adminProvider;
        private readonly ImageProvider _imageProvider;
        private readonly IMemoryCache _cache;
        

        public BlogService(IHostEnvironment environment, BlogProvider blogProvider, CategoryProvider categoryProvider, AdminProvider adminProvider, ImageProvider imageProvider, IMemoryCache cache)
        {
            _blogProvider = blogProvider;
            _categoryProvider = categoryProvider;
            _adminProvider = adminProvider;
            _imageProvider = imageProvider;
            _environment = environment;
            _cache = cache;
        }

        public async Task<Blog> GetById(Guid id)
        {
            if (_cache.TryGetValue(id, out Blog blog)) return blog;
            blog = await _blogProvider.GetById(id);
            if (blog != null)
            {
                _cache.Set(blog.Id, blog,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
            }
            return blog;
        }

        public async Task AddBlog(BlogCreationDto blogViewModel)
        {
            Guid id = Guid.NewGuid();
            Category category = await _categoryProvider.GetById(blogViewModel.Category.Id);
            Admin admin = await _adminProvider.GetById(blogViewModel.AuthorId);
            var blog = new Blog
            {
                Id = id,
                Title = blogViewModel.Title,
                Description = blogViewModel.Description,
                Category =  category,
                Author = admin,
                Tags =  blogViewModel.Tags,
                MainBlogText = blogViewModel.MainBlogText,
                Image = blogViewModel.Image
            };
            
            await _imageProvider.Add(blogViewModel.Image);
            await _blogProvider.Add(blog);
            _cache.Set(blog.Id, blog, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
        }

        public async Task EditBlog(BlogCreationDto blogViewModel, Blog editBlog)
        {
            Category category = await _categoryProvider.GetById(blogViewModel.Category.Id);
            
            editBlog.Title = blogViewModel.Title;
            editBlog.MainBlogText = blogViewModel.MainBlogText;
            editBlog.Description = blogViewModel.Description;
            editBlog.Category = category;
            editBlog.Tags = blogViewModel.Tags;
            try
            {
                Image image = await _imageProvider.GetByWebImagePath(blogViewModel.Image.WebImagePath);
                Console.WriteLine(image);
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception);
                await _imageProvider.Remove(editBlog.Image);
                await _imageProvider.Add(blogViewModel.Image);
                editBlog.Image = blogViewModel.Image;
            }
            
            await _blogProvider.Edit(editBlog);
            _cache.Set(editBlog.Id, editBlog, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
        }

        public async Task DeleteBlog(Blog blog)
        {
            await _blogProvider.Remove(blog);
            _cache.Remove(blog.Id);
        }

        public async Task IncreaseView(Blog blog)
        {
            blog.ViewCount++;
            await _blogProvider.Edit(blog);
            _cache.Set(blog.Id, blog, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
        }

        public async Task SetIsFeatured(Blog blog)
        {
            blog.IsFeatured = !blog.IsFeatured;
            await _blogProvider.Edit(blog);
            _cache.Set(blog.Id, blog, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
        }
    }
}