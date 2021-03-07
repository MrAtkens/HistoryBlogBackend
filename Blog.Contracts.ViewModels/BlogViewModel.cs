using System;
using System.Collections.Generic;
using BazarJok.DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace BazarJok.Contracts.ViewModels
{
    public class BlogViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } // Blog name
        public string Description { get; set; } // Description of blog
        public string MainBlogText { get; set; } // Main text and images in blog
        public Category Category { get; set; }
        public string AuthorName { get; set; }
        public string Tags { get; set; }
        public Image Image { get; set; }
        public string CreationDate { get; set; }
        public int ViewCount { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsAccepted { get; set; }

    }
}