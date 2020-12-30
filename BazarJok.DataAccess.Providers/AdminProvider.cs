using BazarJok.DataAccess.Domain;
using BazarJok.DataAccess.Models;
using BazarJok.DataAccess.Providers.Abstract;
using System;
using System.Threading.Tasks;

namespace BazarJok.DataAccess.Providers
{
    public class AdminProvider : EntityProvider<ApplicationContext, Admin, Guid>
    {
        private readonly ApplicationContext _context;

        public AdminProvider(ApplicationContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<Admin> GetByLogin(string login)
        {
            return await FirstOrDefault(x => x.Login.Equals(login)) ??
                 throw new ArgumentException("No such admin in database");
        }
    }
}
