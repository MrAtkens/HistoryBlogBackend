using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BazarJok.Contracts.Attributes;
using BazarJok.Contracts.Dtos;
using BazarJok.Contracts.Options;
using BazarJok.Contracts.ViewModels;
using BazarJok.DataAccess.Models;
using BazarJok.DataAccess.Providers;
using BazarJok.Services.Business;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Blog.Controllers
{
    [Route("api/quotes/")]
    [ApiController]
    public class QuoteCrud : ControllerBase
    {
        private readonly QuoteService _quoteService;
        private readonly QuoteProvider _quoteProvider;

        public QuoteCrud(QuoteService quoteService, QuoteProvider quoteProvider)
        {
            _quoteService = quoteService;
            _quoteProvider = quoteProvider;
        }

        [HttpGet("{id}")]
        [EnableCors(CorsOrigins.FrontPolicy)]
        public async Task<IActionResult> Get(Guid id)
        {
            var category = await _quoteService.GetById(id);
            if (category is null)
                return NotFound("Quote is not found");
            return Ok(category);
        }

        [HttpGet]
        [EnableCors(CorsOrigins.FrontPolicy)]
        public async Task<IActionResult> GetAll()
        {
            var quotes = await _quoteProvider.GetAllQuotes();
            var quoteViewModels = quotes.Select(quote=> new QuoteViewModel() 
            {
                Id = quote.Id, 
                FullName = quote.FullName,
                Description = quote.Description,
                Date = quote.Date,
                Image = quote.Image,
                CreationDate = quote.CreationDate.ToString("d")
            }).ToList();
            return Ok(quoteViewModels);
        }

        [HttpPost]
        [EnableCors(CorsOrigins.AdminPanelPolicy)]
        [AdminAuthorized(roles:AdminRole.Admin)]
        public async Task<IActionResult> Post(QuoteDto quoteDto)
        {
            try
            {
                await _quoteService.AddQuote(quoteDto);
                return NoContent();
            }
            catch (Exception exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        [EnableCors(CorsOrigins.AdminPanelPolicy)]
        [AdminAuthorized(roles:AdminRole.Admin)]
        public async Task<IActionResult> Update(Guid id, QuoteDto quoteDto)
        {
            try
            {
                var blog = await _quoteProvider.GetById(id);
                if (blog is null)
                    return NotFound("Quote is not found");
                await _quoteService.EditQuote(quoteDto, blog); // blog = Blog class
                return NoContent();
            }
            catch (Exception exception)
            {
                return StatusCode(500);
            }
        }
        
        [HttpDelete("{id}")]
        [EnableCors(CorsOrigins.AdminPanelPolicy)]
        [AdminAuthorized(roles:AdminRole.Admin)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var quote = await _quoteProvider.GetById(id);
            if (quote is null)
                return NotFound("Quote is not found");
            await _quoteService.DeleteQuote(quote);
            return NoContent();
        }

    }

}