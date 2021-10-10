using GeekBlog.DataAccess.Domain;
using GeekBlog.DataAccess.Models;
using GeekBlog.DataAccess.Providers.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GeekBlog.DataAccess.Providers
{
    public class ImageProvider : EntityProvider<ApplicationContext, Image, Guid>
    {
        private readonly ApplicationContext _context;

        public ImageProvider(ApplicationContext context) : base(context)
        {
            this._context = context;
        }
        
        public async Task<Image> GetByWebImagePath(string name)
        {
            return await FirstOrDefault(x => x.WebImagePath.Equals(name)) ??
                   throw new ArgumentException("No such image in database");
        }
      
    }
}