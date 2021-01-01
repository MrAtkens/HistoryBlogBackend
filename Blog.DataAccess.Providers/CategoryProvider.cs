using System;
using System.Threading.Tasks;
using BazarJok.Contracts.Dtos;
using BazarJok.DataAccess.Domain;
using BazarJok.DataAccess.Models;
using BazarJok.DataAccess.Providers.Abstract;

namespace BazarJok.DataAccess.Providers
{
    public abstract class CategoryProvider : EntityProvider<ApplicationContext, Category, Guid>
    {
        private readonly ApplicationContext _context;

        protected CategoryProvider(ApplicationContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Category> GetByName(string name)
        {
            var user = await FirstOrDefault(x =>
                x.Name.ToLower().Equals(name.ToLower()));

            return user ?? throw new ArgumentException("BazarJok.Api.Category is not found");
        }

        public abstract Task Add(CategoryCreationDto categoryCreationDto);
    }
}