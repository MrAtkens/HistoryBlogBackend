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

        public async Task<List<Blog>> GetLatestBlog()
        {
            var blogs = await _blogProvider.GetLatestBlogs();
            List<Blog> latestBlogs = new List<Blog>();
            if (blogs.Count < 6)
            {
                latestBlogs.Add(blogs[0]);
                latestBlogs.Add(blogs[1]);
                latestBlogs.Add(blogs[2]);
            }
            else
            {
                latestBlogs.Add(blogs[0]);
                latestBlogs.Add(blogs[1]);
                latestBlogs.Add(blogs[2]);
                latestBlogs.Add(blogs[3]);
                latestBlogs.Add(blogs[4]);
                latestBlogs.Add(blogs[5]);
            }

            return latestBlogs;
        }

        public async Task AddBlog(BlogCreationDto blogViewModel)
        {
            Guid id = Guid.NewGuid();
            Category category = await _categoryProvider.GetById(blogViewModel.Category.Id);
            Admin admin = await _adminProvider.GetById(blogViewModel.AuthorId);
            Image imageNew = new Image();
            imageNew.Alt = blogViewModel.Image.Alt;
            imageNew.ImageName = blogViewModel.Image.ImageName;
            imageNew.WebImagePath = blogViewModel.Image.WebImagePath;
            var blog = new Blog
            {
                Id = id,
                Title = blogViewModel.Title,
                Description = blogViewModel.Description,
                Category =  category,
                Author = admin,
                Tags =  blogViewModel.Tags,
                MainBlogText = blogViewModel.MainBlogText,
                Image = imageNew
            };
            
            await _imageProvider.Add(imageNew);
            await _blogProvider.Add(blog);
            _cache.Set(blog.Id, blog, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
        }

        public async Task EditBlog(BlogCreationDto blogCreationDto, Blog editBlog)
        {
            Category category = await _categoryProvider.GetById(blogCreationDto.Category.Id);
            
            editBlog.Title = blogCreationDto.Title;
            editBlog.MainBlogText = blogCreationDto.MainBlogText;
            editBlog.Description = blogCreationDto.Description;
            editBlog.Category = category;
            editBlog.Tags = blogCreationDto.Tags;
            try
            {
                Image image = await _imageProvider.GetByWebImagePath(blogCreationDto.Image.WebImagePath);
                if (image.Equals(null))
                {
                    _cache.Remove(editBlog.Id);
                    Image imageNew = new Image();
                    imageNew.Alt = blogCreationDto.Image.Alt;
                    imageNew.ImageName = blogCreationDto.Image.ImageName;
                    imageNew.WebImagePath = blogCreationDto.Image.WebImagePath;
                    await _imageProvider.Remove(editBlog.Image);
                    await _imageProvider.Add(imageNew);
                    editBlog.Image = imageNew;
                }
                else if (blogCreationDto.Image.Alt != image.Alt)
                {
                    image.Alt = blogCreationDto.Image.Alt;
                    await _imageProvider.Edit(image);
                    editBlog.Image = image;
                }
            }
            catch(Exception exception)
            {
                _cache.Remove(editBlog.Id);
                Image imageNew = new Image();
                imageNew.Alt = blogCreationDto.Image.Alt;
                imageNew.ImageName = blogCreationDto.Image.ImageName;
                imageNew.WebImagePath = blogCreationDto.Image.WebImagePath;
                await _imageProvider.Remove(editBlog.Image);
                await _imageProvider.Add(imageNew);
                editBlog.Image = imageNew;
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