using System;
using System.Collections.Generic;
using System.Text;

namespace GeekBlog.DataAccess.Models
{
    public class Image : Entity
    {
        public Image(string imageName, string alt, string webImagePath)
        {
            ImageName = imageName;
            Alt = alt;
            WebImagePath = webImagePath;
        }
        public string ImageName { get; set; }
        public string Alt { get; set; }
        public string WebImagePath { get; set; }
    }
}
