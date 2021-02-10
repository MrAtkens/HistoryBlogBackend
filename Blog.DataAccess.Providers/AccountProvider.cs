using System;
using System.Threading.Tasks;
using BazarJok.Contracts.Dtos;
using BazarJok.DataAccess.Domain;
using BazarJok.DataAccess.Models;
using BazarJok.DataAccess.Providers.Abstract;

namespace BazarJok.DataAccess.Providers
{
    public abstract class AccountProvider<T> : EntityProvider<ApplicationContext, T, Guid> where T: User
    {
        private readonly ApplicationContext _context;

        protected AccountProvider(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<T> GetByEmail(string email)
        {
            var user = await this.FirstOrDefault(x => 
                x.Email.ToLower().Equals(email.ToLower()));

            return user ?? throw new ArgumentException("User is not found");
        }

        public abstract Task Add(UserCreationDto user);
    }
}