using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BazarJok.Contracts.ViewModels;
using BazarJok.DataAccess.Models;
using BazarJok.DataAccess.Providers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;

namespace BazarJok.Services.Business
{
    public class BlogService
    {
        private readonly IHostingEnvironment _environment;
        private readonly BlogProvider _blogProvider;
        private readonly IMemoryCache _cache;
        

        public BlogService(IHostingEnvironment environment, BlogProvider blogProvider, IMemoryCache cache)
        {
            _blogProvider = blogProvider;
            _environment = environment;
            _cache = cache;
        }
        
        public async Task AddBlog(BlogViewModel blogViewModel)
        {
            Guid id = Guid.NewGuid();
            List<Image> uploadedImages = new List<Image>();
            var blog = new Blog
            {
                Id = id,
                Title = blogViewModel.Title,
                Description = blogViewModel.Description,
                Categories =  blogViewModel.Categories,
                KeyWords =  blogViewModel.KeyWords,
                MainBlogText = blogViewModel.MainBlogText
            };
            
            foreach (var file in blogViewModel.Files)
            {
                FileInfo fileInfo = new FileInfo(file.FileName);
                string fileName = Guid.NewGuid() + fileInfo.Extension;
                if (!Directory.Exists(_environment.ContentRootPath + "\\BlogImages\\" + $"\\{id}\\"))
                {
                    Directory.CreateDirectory(_environment.ContentRootPath + "\\BlogImages\\" + $"\\{id}\\");
                }
                FileStream filestream = File.Create(_environment.ContentRootPath + "\\BlogImages\\" + $"\\{id}\\" + fileName);
                await file.CopyToAsync(filestream);
                await filestream.FlushAsync();
                Image image = new Image
                {
                    Alt = fileName,
                    ImagePath = $"\\PinPublicImages\\{id}\\{fileName}",
                    WebImagePath = "https://localhost:5201/PinPublicImages/" + id + "/" + fileName,
                };
                uploadedImages.Add(image);

            }

            blog.Images = uploadedImages;

            await _blogProvider.Add(blog);
            _cache.Set(blog.Id, blog, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
        }

        public async Task EditBlog(BlogViewModel blogViewModel, Blog editBlog)
        {
            List<Image> uploadedImages = new List<Image>();
            foreach (var file in blogViewModel.Files)
            {
                FileInfo fileInfo = new FileInfo(file.FileName);
                string fileName = Guid.NewGuid() + fileInfo.Extension;
                if (!Directory.Exists(_environment.ContentRootPath + "\\BlogImages\\" + $"\\{editBlog.Id}\\"))
                {
                    Directory.CreateDirectory(_environment.ContentRootPath + "\\BlogImages\\" + $"\\{editBlog.Id}\\");
                }
                FileStream filestream = File.Create(_environment.ContentRootPath + "\\BlogImages\\" + $"\\{editBlog.Id}\\" + fileName);
                await file.CopyToAsync(filestream);
                await filestream.FlushAsync();
                Image image = new Image
                {
                    Alt = fileName,
                    ImagePath = _environment.ContentRootPath + "\\BlogImages\\" + $"\\{editBlog.Id}\\" + fileName,
                    WebImagePath = "https://localhost:5201/PinPublicImages/" + editBlog.Id + "/" + fileName,
                };
                uploadedImages.Add(image);

            }
            editBlog.Title = blogViewModel.Title;
            editBlog.MainBlogText = blogViewModel.MainBlogText;
            editBlog.Description = blogViewModel.Description;
            editBlog.Categories = blogViewModel.Categories;
            editBlog.KeyWords = blogViewModel.KeyWords;
            editBlog.Images = uploadedImages;
            await _blogProvider.Edit(editBlog);
        }

        public async Task DeleteBlog(Blog blog)
        {
            foreach (var file in blog.Images)
            {
                if (File.Exists(file.ImagePath))
                {
                    File.Delete(file.ImagePath);
                }
            }

            await _blogProvider.Remove(blog);
        }
    }
}