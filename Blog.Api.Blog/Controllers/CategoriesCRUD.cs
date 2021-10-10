using System;
using System.Linq;
using System.Threading.Tasks;
using GeekBlog.Contracts.Attributes;
using GeekBlog.Contracts.Dtos;
using GeekBlog.Contracts.Options;
using GeekBlog.Contracts.ViewModels;
using GeekBlog.DataAccess.Models;
using GeekBlog.DataAccess.Providers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Blog.Controllers
{
    [Route("api/categories/")]
    [ApiController]
    public class CategoriesCrud : ControllerBase
    {
        private readonly CategoryProvider _categoryProvider;
        private readonly ImageProvider _imageProvider;

        public CategoriesCrud(CategoryProvider categoryProvider, ImageProvider imageProvider)
        {
            _categoryProvider = categoryProvider;
            _imageProvider = imageProvider;
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
            var categories = await _categoryProvider.GetAllCategory();
            var categoryViewModels = 
                categories.Select(category=> new CategoryViewModel 
                {
                    Id = category.Id, 
                    Name = category.Name, 
                    Description = category.Description,
                    SortIndex = category.SortIndex,
                    Image =  category.Image,
                    CreationDate = category.CreationDate.ToString("d")
                }).ToList();

            return Ok(categoryViewModels);
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
                SortIndex = categoryCreationDto.SortIndex,
                Image = new Image(categoryCreationDto.Image.ImageName, categoryCreationDto.Image.Alt, categoryCreationDto.Image.WebImagePath)
            };
            await _imageProvider.Add(category.Image);
            await _categoryProvider.Add(category);
            return Ok();
        }

        [HttpPut("{id}")]
        [EnableCors(CorsOrigins.AdminPanelPolicy)]
        [AdminAuthorized(roles:AdminRole.Admin)]
        public async Task<IActionResult> Update(Guid id, CategoryCreationDto categoryCreationDto)
        {
            var category = await _categoryProvider.GetById(id);
            if (category is null )
                return NotFound("Category is not found");
            var image = await _imageProvider.GetById(category.Image.Id);
            if (image is null)
                return NotFound("Image is not found");
            //Edit category
            category.Name = categoryCreationDto.Name;
            category.Description = categoryCreationDto.Description;
            category.SortIndex = categoryCreationDto.SortIndex;
            //Image edit
            image.ImageName = categoryCreationDto.Image.ImageName;
            image.WebImagePath = categoryCreationDto.Image.WebImagePath;
            image.Alt = categoryCreationDto.Image.Alt;
            //Edit image in category
            category.Image = image;
            //Provider edit
            await _imageProvider.Edit(image);
            await _categoryProvider.Edit(category);
            return Ok();
        }
        
        [HttpDelete("{id}")]
        [EnableCors(CorsOrigins.AdminPanelPolicy)]
        [AdminAuthorized(roles:AdminRole.Admin)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var category = await _categoryProvider.GetById(id);
            if (category is null)
                return NotFound("Category is not found");
            await _imageProvider.Remove(category.Image);
            await _categoryProvider.Remove(category);
            return Ok();
        }

    }

}