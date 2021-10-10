using BazarJok.DataAccess.Models.System;
using System.Collections.Generic;

namespace GeekBlog.DataAccess.Models
{
    public class Blog : Entity
    {
        public string Title { get; set; } // Blog name
        public string Description { get; set; } // Description of blog
        public string MainBlogText { get; set; } // HTML component created by editors
        public Admin Author { get; set; }
        public Category Category { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public Image Image { get; set; }
        public bool IsFeatured { get; set; } = false;
        public bool IsAccepted { get; set; } = false;
        public int ViewCount { get; set; } = 0;

        public void addViewCount()
        {
            ViewCount++;
        }
    }
}
