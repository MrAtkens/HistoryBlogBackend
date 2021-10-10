using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GeekBlog.Contracts.Dtos;
using GeekBlog.Contracts.ViewModels;
using GeekBlog.DataAccess.Models;
using GeekBlog.DataAccess.Providers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;

namespace GeekBlog.Services.Business
{
    public class BlogService
    {
        private readonly BlogProvider _blogProvider;
        private readonly CategoryProvider _categoryProvider;
        private readonly EntityAdminProvider _adminProvider;
        private readonly ImageProvider _imageProvider;
        private readonly IMemoryCache _cache;
        

        public BlogService(BlogProvider blogProvider, CategoryProvider categoryProvider, EntityAdminProvider adminProvider, ImageProvider imageProvider, IMemoryCache cache)
        {
            _blogProvider = blogProvider;
            _categoryProvider = categoryProvider;
            _adminProvider = adminProvider;
            _imageProvider = imageProvider;
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
        
        public async Task<Blog> GetByIdAdmin(Guid id)
        {
            if (_cache.TryGetValue(id, out Blog blog)) return blog;
            blog = await _blogProvider.GetByIdAdmin(id);
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
            Image imageNew = new Image(blogViewModel.Image.ImageName, blogViewModel.Image.Alt, blogViewModel.Image.WebImagePath);
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
                    Image imageNew = new Image(blogCreationDto.Image.ImageName, blogCreationDto.Image.Alt, blogCreationDto.Image.WebImagePath);
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
                Image imageNew = new Image(blogCreationDto.Image.ImageName, blogCreationDto.Image.Alt, blogCreationDto.Image.WebImagePath);
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
        
        public async Task SetIsAccepted(Blog blog)
        {
            blog.IsAccepted = !blog.IsAccepted;
            await _blogProvider.Edit(blog);
            _cache.Set(blog.Id, blog, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
        }
    }
}