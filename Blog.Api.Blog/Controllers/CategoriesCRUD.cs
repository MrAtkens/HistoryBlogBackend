using System;
using System.Threading.Tasks;
using BazarJok.Contracts.Attributes;
using BazarJok.Contracts.Dtos;
using BazarJok.Contracts.Options;
using BazarJok.Contracts.ViewModels;
using BazarJok.DataAccess.Models;
using BazarJok.DataAccess.Providers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Blog.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/category/")]
    [ApiController]
    public class CategoriesCrud : ControllerBase
    {
        private readonly CategoryProvider _categoryProvider;

        public CategoriesCrud(CategoryProvider categoryProvider)
        {
            _categoryProvider = categoryProvider;
        }

        [HttpGet("{id}")]
        [EnableCors(CorsOrigins.FrontPolicy)]
        public async Task<IActionResult> Get(Guid id)
        {
            var category = await _categoryProvider.GetById(id);
            if (category is null)
                return NotFound("Category is not found");
            return Ok(category);
        }

        [HttpGet]
        [EnableCors(CorsOrigins.FrontPolicy)]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryProvider.GetAll();
            return Ok(categories);
        }

        [HttpPost]
        [EnableCors(CorsOrigins.AdminPanelPolicy)]
        [AdminAuthorized(roles:AdminRole.Admin)]
        public async Task<IActionResult> Post(CategoryCreationDto categoryCreationDto)
        {
            var category = new Category
            {
                Name = categoryCreationDto.Name,
                Description = categoryCreationDto.Description,
                SortIndex = categoryCreationDto.SortIndex
            };

            await _categoryProvider.Add(category);
            return Ok();
        }

        [HttpPut("{id}")]
        [EnableCors(CorsOrigins.AdminPanelPolicy)]
        [AdminAuthorized(roles:AdminRole.Admin)]
        public async Task<IActionResult> Update(Guid id, CategoryCreationDto categoryCreationDto)
        {
            var category = await _categoryProvider.GetById(id);
            if (category is null)
                return NotFound("Category is not found");

            category.Name = categoryCreationDto.Name;
            category.Description = categoryCreationDto.Description;
            category.SortIndex = categoryCreationDto.SortIndex;
            await _categoryProvider.Edit(category);
            return Ok();
        }
        
        [HttpDelete("{id}")]
        [ValidateAntiForgeryToken]
        [EnableCors(CorsOrigins.AdminPanelPolicy)]
        [AdminAuthorized(roles:AdminRole.Admin)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var category = await _categoryProvider.GetById(id);
            if (category is null)
                return NotFound("Category is not found");
            
            await _categoryProvider.Remove(category);
            return Ok();
        }

    }

}