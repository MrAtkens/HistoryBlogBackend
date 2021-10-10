using BazarJok.DataAccess.Providers.Abstract;
using GeekBlog.DataAccess.Domain;
using GeekBlog.DataAccess.Models;
using GeekBlog.DataAccess.Providers.Abstract;
using System;
using System.Threading.Tasks;

namespace GeekBlog.DataAccess.Providers
{
    public class EntityAdminProvider : EntityProvider<ApplicationContext, Admin, Guid>, IAdminProvider
    {
        public EntityAdminProvider(ApplicationContext context) : base(context)
        {
        }

        public async Task<Admin> GetByLogin(string login)
        {
            return await FirstOrDefault(x => x.Login == login) ?? throw new ArgumentException();
        }

    }

}
