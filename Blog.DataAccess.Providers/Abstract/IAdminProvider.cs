using GeekBlog.DataAccess.Models;
using GeekBlog.DataAccess.Providers.Abstract.Base;
using System.Threading.Tasks;
using System;

namespace BazarJok.DataAccess.Providers.Abstract
{
    public interface IAdminProvider : IProvider<Admin, Guid>
    {
        Task<Admin> GetByLogin(string login);
    }

}
