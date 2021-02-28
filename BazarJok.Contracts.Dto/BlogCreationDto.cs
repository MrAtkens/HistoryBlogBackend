using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BazarJok.DataAccess.Models;

namespace BazarJok.Contracts.Dtos
{
    public class BlogCreationDto
    {
        [Required]
        public string Title { get; set; } // Blog name
        [Required]
        public string Description { get; set; } // Description of blog
        [Required]
        public string MainBlogText { get; set; } // Main text and images in blog
        [Required]
        public Guid AuthorId { get; set; }
        [Required]
        public Category Category { get; set; }
        [Required] 
        public string Tags { get; set; }
        
        [Required]
        public ImageDto Image { get; set; }
    }
}