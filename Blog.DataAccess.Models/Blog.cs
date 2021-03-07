using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BazarJok.DataAccess.Models
{
    public class Blog : Entity
    {
        public string Title { get; set; } // Blog name
        public string Description { get; set; } // Description of blog
        public string MainBlogText { get; set; } // Main text and images in blog
        public Admin Author { get; set; }
        public Category Category { get; set; }
        public String Tags { get; set; }
        public Image Image { get; set; }
        public bool IsFeatured { get; set; } = false;
        public bool IsAccepted { get; set; } = false;
        public int ViewCount { get; set; } = 0;
    }
}
