using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BazarJok.DataAccess.Domain;
using BazarJok.DataAccess.Models;
using BazarJok.DataAccess.Providers.Abstract;
using Microsoft.EntityFrameworkCore;

namespace BazarJok.DataAccess.Providers
{
    public class QuoteProvider : EntityProvider<ApplicationContext, Quote, Guid>
    {
        private readonly ApplicationContext _context;

        public QuoteProvider(ApplicationContext context) : base(context)
        {
            this._context = context;
        }
        
        public new async Task<Quote> GetById(Guid id)
        {
            return await _context.Quotes.Include(blog => blog.Image).FirstOrDefaultAsync(category => category.Id == id);
        }
        
        public async Task<List<Quote>> GetAllQuotes()
        {
            return await _context.Quotes.Include(quote => quote.Image).ToListAsync();
        }
    }
}