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
        private readonly ImageProvider _imageProvider;
        private readonly IMemoryCache _cache;
        

        public BlogService(IHostEnvironment environment, BlogProvider blogProvider, CategoryProvider categoryProvider, ImageProvider imageProvider, IMemoryCache cache)
        {
            _blogProvider = blogProvider;
            _categoryProvider = categoryProvider;
            _imageProvider = imageProvider;
            _environment = environment;
            _cache = cache;
        }
        
        public async Task AddBlog(BlogCreationDto blogViewModel)
        {
            Guid id = Guid.NewGuid();
            Category category = await _categoryProvider.GetById(blogViewModel.Category.Id);
            var blog = new Blog
            {
                Id = id,
                Title = blogViewModel.Title,
                Description = blogViewModel.Description,
                Category =  category,
                Tags =  blogViewModel.Tags,
                MainBlogText = blogViewModel.MainBlogText,
                Image = blogViewModel.Image
            };
            
            await _imageProvider.Add(blogViewModel.Image);
            await _blogProvider.Add(blog);
            _cache.Set(blog.Id, blog, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
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
        }

        public async Task DeleteBlog(Blog blog)
        {
            await _blogProvider.Remove(blog);
        }
    }
}