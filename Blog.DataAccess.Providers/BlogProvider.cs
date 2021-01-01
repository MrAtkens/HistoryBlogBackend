using System;
using System.Threading.Tasks;
using BazarJok.Contracts.Dtos;
using BazarJok.DataAccess.Domain;
using BazarJok.DataAccess.Models;
using BazarJok.DataAccess.Providers.Abstract;

namespace BazarJok.DataAccess.Providers
{
    public abstract class BlogProvider : EntityProvider<ApplicationContext, Blog, Guid>
    {
        private readonly ApplicationContext _context;

        protected BlogProvider(ApplicationContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Blog> GetByTitle(string name)
        {
            var user = await FirstOrDefault(x =>
                x.Title.ToLower().Equals(name.ToLower()));

            return user ?? throw new ArgumentException("BazarJok.Api.Blog is not found");
        }

        public new abstract Task Add(Blog blogDto);
    }
}