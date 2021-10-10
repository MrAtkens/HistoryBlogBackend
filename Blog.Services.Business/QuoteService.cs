using System;
using System.Threading.Tasks;
using GeekBlog.Contracts.Dtos;
using GeekBlog.DataAccess.Models;
using GeekBlog.DataAccess.Providers;
using Microsoft.Extensions.Caching.Memory;

namespace GeekBlog.Services.Business
{
    public class QuoteService
    {
        private readonly IMemoryCache _cache;
        private readonly QuoteProvider _quoteProvider;
        private readonly ImageProvider _imageProvider;
        

        public QuoteService(QuoteProvider quoteProvider, ImageProvider imageProvider, IMemoryCache cache)
        {
            _quoteProvider = quoteProvider;
            _imageProvider = imageProvider;
            _cache = cache;
        }

        public async Task<Quote> GetById(Guid id)
        {
            if (_cache.TryGetValue(id, out Quote quote)) return quote;
            quote = await _quoteProvider.GetById(id);
            if (quote != null)
            {
                _cache.Set(quote.Id, quote,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
            }
            return quote;
        }

        public async Task AddQuote(QuoteDto quoteDto)
        {
            Image imageNew = new Image(quoteDto.ImageDto.ImageName, quoteDto.ImageDto.Alt, quoteDto.ImageDto.WebImagePath);
            Quote quote = new Quote();
            quote.FullName = quoteDto.FullName;
            quote.Description = quoteDto.Description;
            quote.Date = quoteDto.Date;
            quote.Image = imageNew;
            await _imageProvider.Add(imageNew);
            await _quoteProvider.Add(quote);
            _cache.Set(quote.Id, quote, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
        }

        public async Task EditQuote(QuoteDto quote, Quote editQuote)
        {
            editQuote.FullName = quote.FullName;
            editQuote.Description = quote.Description;
            editQuote.Date = quote.Date;
            try
            {
                Image image = await _imageProvider.GetByWebImagePath(quote.ImageDto.WebImagePath);
                if (image.Equals(null))
                {
                    _cache.Remove(editQuote.Id);
                    Image imageNew = new Image(quote.ImageDto.ImageName, quote.ImageDto.Alt, quote.ImageDto.WebImagePath);
                    await _imageProvider.Remove(editQuote.Image);
                    await _imageProvider.Add(imageNew);
                    editQuote.Image = imageNew;
                }
                else if (quote.ImageDto.Alt != image.Alt)
                {
                    image.Alt = quote.ImageDto.Alt;
                    await _imageProvider.Edit(image);
                    editQuote.Image = image;
                }
            }
            catch(Exception exception)
            {
                _cache.Remove(editQuote.Id);
                Image imageNew = new Image(quote.ImageDto.ImageName, quote.ImageDto.Alt, quote.ImageDto.WebImagePath);
                await _imageProvider.Remove(editQuote.Image);
                await _imageProvider.Add(imageNew);
                editQuote.Image = imageNew;
            }
            
            await _quoteProvider.Edit(editQuote);
            _cache.Set(editQuote.Id, editQuote, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
        }

        public async Task DeleteQuote(Quote quote)
        {
            await _quoteProvider.Remove(quote);
            _cache.Remove(quote.Id);
        }
    }
}