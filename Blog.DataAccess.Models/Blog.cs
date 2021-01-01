using System;
using System.Collections.Generic;
using System.Text;

namespace BazarJok.DataAccess.Models
{
    public class Blog : Entity
    {
        public string Title { get; set; } // Blog name
        public string Description { get; set; } // Description of blog
        public string MainBlogText { get; set; } // Main text and images in blog
        public List<Category> Categories { get; set; }
        public List<string> KeyWords { get; set; }
        public List<Image> Images { get; set; }
        public Image Image { get; set; }
    }
}
