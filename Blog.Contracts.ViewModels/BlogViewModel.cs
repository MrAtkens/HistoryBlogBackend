using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using BazarJok.DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace BazarJok.Contracts.ViewModels
{
    public class BlogViewModel
    {
        [Required]
        public string Title { get; set; } // Blog name
        [Required]
        public string Description { get; set; } // Description of blog
        [Required]
        public string MainBlogText { get; set; } // Main text and images in blog
        [Required]
        public List<Category> Categories { get; set; }
        [Required] 
        public List<string> KeyWords { get; set; }
        
        [Required]
        public List<IFormFile> Files { get; set; }
    }
}