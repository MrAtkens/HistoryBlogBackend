using BazarJok.DataAccess.Domain;
using BazarJok.DataAccess.Models;
using BazarJok.DataAccess.Providers.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BazarJok.Contracts.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BazarJok.DataAccess.Providers
{
    public class BlogProvider : EntityProvider<ApplicationContext, Blog, Guid>
    {
        private readonly ApplicationContext _context;
        
        public BlogProvider(ApplicationContext context) : base(context)
        {
            this._context = context;
        }
        
        public async Task<Blog> GetById(Guid id)
        {
            return await _context.Blogs.Include(blog => blog.Image).Include(blog => blog.Author).Include(blog => blog.Category).FirstOrDefaultAsync(category => category.Id == id);
        }

        public async Task<List<Blog>> GetAllBlogs()
        {
            return await _context.Blogs.Include(blog => blog.Image).Include(blog => blog.Author).Include(blog => blog.Category).ToListAsync();
        }
        
        public async Task<List<Blog>> GetBlogsByPage(BlogsByPage pages)
        {
            if (pages.Page == 1)
            {
                return await _context.Blogs.Include(blog => blog.Image).Include(blog => blog.Author).Include(blog => blog.Category).OrderBy(blog => blog.Title).Take(pages.CountPerPage).ToListAsync();
            }
            return await _context.Blogs.Include(blog => blog.Image).Include(blog => blog.Author).Include(blog => blog.Category).OrderBy(blog => blog.Title).Skip(pages.CountPerPage * (pages.Page-1)).Take(pages.CountPerPage).ToListAsync();
        }
        
        public async Task<int> GetPageCount()
        {
            return await _context.Blogs.CountAsync();
        }
        
        public async Task<List<Blog>> GetLatestBlogs()
        {
            return await _context.Blogs.Include(blog => blog.Image).Include(blog => blog.Author).Include(blog => blog.Category).OrderBy(blog => blog.CreationDate).Take(6).ToListAsync();
        }

        public async Task<List<Blog>> GetFeaturedBlogs()
        {
            return await _context.Blogs.Include(blog => blog.Image).Include(blog => blog.Author).Include(blog => blog.Category).Where(blog => blog.IsFeatured.Equals(true)).ToListAsync();
        }

        public async Task<List<Blog>> GetAllBlogsByTag(string tag)
        {
            return await _context.Blogs.Include(blog => blog.Image).Include(blog => blog.Author).Include(blog => blog.Category).Where(blog => blog.Tags.Contains(tag)).ToListAsync();
        }
        
        public async Task<List<Blog>> GetAllBlogsByCategory(string category)
        {
            return await _context.Blogs.Include(blog => blog.Image).Include(blog => blog.Author).Include(blog => blog.Category).Where(blog => blog.Category.Name.Equals(category)).ToListAsync();
        }

        public async Task<List<Blog>> GetAllBlogsByTitle(BlogFindDto title)
        {
            return await _context.Blogs.Include(blog => blog.Image).Include(blog => blog.Author).Include(blog => blog.Category).Where(blog => blog.Title.Contains(title.Line)).ToListAsync();
        }
        
        public async Task<List<Blog>> GetBlogsByAuthor(Admin admin)
        {
            return await _context.Blogs.Include(blog => blog.Image).Include(blog => blog.Author).Include(blog => blog.Category).Where(blog => blog.Author == admin).ToListAsync();
        }
    }
}