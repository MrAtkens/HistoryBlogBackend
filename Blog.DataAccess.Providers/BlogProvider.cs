using BazarJok.DataAccess.Domain;
using BazarJok.DataAccess.Models;
using BazarJok.DataAccess.Providers.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<Blog> GetByTitle(string title)
        {
            return await FirstOrDefault(x => x.Title.Equals(title)) ??
                   throw new ArgumentException("No such blog in database");
        }
    }
}