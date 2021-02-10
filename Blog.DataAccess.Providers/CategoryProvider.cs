using BazarJok.DataAccess.Domain;
using BazarJok.DataAccess.Models;
using BazarJok.DataAccess.Providers.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BazarJok.DataAccess.Providers
{
    public class CategoryProvider : EntityProvider<ApplicationContext, Category, Guid>
    {
        private readonly ApplicationContext _context;

        public CategoryProvider(ApplicationContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<Category> GetById(Guid id)
        {
            return await _context.Categories.Include(category => category.Image).FirstOrDefaultAsync(category => category.Id == id);
        }

        public async Task<List<Category>> GetAllCategory()
        {
            return await _context.Categories.Include(category => category.Image).ToListAsync();
        }

        public async Task<Category> GetByName(string name)
        {
            return await FirstOrDefault(x => x.Name.Equals(name)) ??
                   throw new ArgumentException("No such category in database");
        }
    }
}