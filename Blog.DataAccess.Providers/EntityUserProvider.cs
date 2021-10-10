using System;
using System.Linq;
using System.Threading.Tasks;
using BazarJok.DataAccess.Providers.Abstract;
using GeekBlog.Contracts.Dtos;
using GeekBlog.DataAccess.Domain;
using GeekBlog.DataAccess.Models;
using GeekBlog.DataAccess.Providers.Abstract;
namespace GeekBlog.DataAccess.Providers
{
    public class EntityUserProvider : EntityProvider<ApplicationContext, User, Guid>, IUserProvider
    {
        private readonly ApplicationContext _context;

        public EntityUserProvider(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetByEmailOrPhone(string emailOrPhone)
        {
            var user = await Get(x =>
                x.Email.ToLower().Equals(emailOrPhone.ToLower()));
            return user.FirstOrDefault() ?? throw new ArgumentException("User is not found");
        }

        public async Task Add(UserCreationDto user)
        {
            await Add(new User
            {
                Email = user.Email,
                PasswordHash = user.Password
            });
        }
    }

}