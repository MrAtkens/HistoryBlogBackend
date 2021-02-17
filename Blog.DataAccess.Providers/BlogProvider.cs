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
            return await _context.Blogs.Include(blog => blog.Image).Include(blog => blog.Category).FirstOrDefaultAsync(category => category.Id == id);
        }

        public async Task<List<Blog>> GetAllBlogs()
        {
            return await _context.Blogs.Include(blog => blog.Image).Include(blog => blog.Category).ToListAsync();
        }

        public async Task<List<Blog>> GetAllBlogsByTag(BlogFindDto tag)
        {
            return await _context.Blogs.Include(blog => blog.Image).Include(blog => blog.Category).Where(blog => blog.Tags.Contains(tag.Line)).ToListAsync();
        }
        
        public async Task<List<Blog>> GetAllBlogsByCategory(BlogFindDto category)
        {
            return await _context.Blogs.Include(blog => blog.Image).Include(blog => blog.Category).Where(blog => blog.Category.Name.Equals(category.Line)).ToListAsync();
        }

        public async Task<List<Blog>> GetAllBlogsByTitle(BlogFindDto title)
        {
            return await _context.Blogs.Include(blog => blog.Image).Include(blog => blog.Category).Where(blog => blog.Title.Contains(title.Line)).ToListAsync();
        }
    }
}