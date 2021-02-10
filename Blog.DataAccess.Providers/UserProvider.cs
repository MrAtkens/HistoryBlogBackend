using System;
using System.Threading.Tasks;
using BazarJok.Contracts.Dtos;
using BazarJok.DataAccess.Domain;
using BazarJok.DataAccess.Models;
using BazarJok.DataAccess.Providers.Abstract;
namespace BazarJok.DataAccess.Providers
{
    public class UserProvider: AccountProvider<User>
    {
        private readonly ApplicationContext _context;
        public UserProvider(ApplicationContext context) : base(context)
        {
            _context = context;
        }
        public override async Task Add(UserCreationDto user)
        {
            await this.Add(new User
            {
                Email = user.Email,
                PasswordHash = user.Password
            });
        }
    }
}