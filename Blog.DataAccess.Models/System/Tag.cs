using GeekBlog.DataAccess.Models;
using System.Collections.Generic;

namespace BazarJok.DataAccess.Models.System
{
    public class Tag : Entity
    {
        public string Title { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
    }
}
