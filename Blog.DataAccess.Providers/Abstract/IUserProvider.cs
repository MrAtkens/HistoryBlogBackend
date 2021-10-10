using GeekBlog.DataAccess.Providers.Abstract.Base;
using GeekBlog.DataAccess.Models;
using GeekBlog.Contracts.Dtos;
using System.Threading.Tasks;
using System;

namespace BazarJok.DataAccess.Providers.Abstract
{
    public interface IUserProvider : IProvider<User, Guid>
    {
        Task<User> GetByEmailOrPhone(string emailOrPhone);
        Task<User> GetBySubscribe();
        Task Add(UserCreationDto user);
    }

}
