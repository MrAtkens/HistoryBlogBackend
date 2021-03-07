using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BazarJok.Contracts.ViewModels;
using BazarJok.DataAccess.Models;

namespace BazarJok.Contracts.Options
{
    public class BlogGetHelper
    {
        public static List<BlogViewModel> GetBlogsViewModel(List<Blog> blogs)
        {
            return blogs.Select(blog=> new BlogViewModel() 
            {
                Id = blog.Id, 
                Title = blog.Title,
                Description = blog.Description,
                AuthorName = blog.Author.Login,
                Tags = blog.Tags,
                Category = blog.Category,
                MainBlogText = blog.MainBlogText,
                Image = blog.Image,
                CreationDate = blog.CreationDate.ToString("dddd dd MMMM yyyy", 
                    CultureInfo.CreateSpecificCulture("ru-RU")),
                IsFeatured = blog.IsFeatured,
                IsAccepted = blog.IsAccepted,
                ViewCount = blog.ViewCount
            }).ToList();
        }

    }
}