using System;
using GeekBlog.DataAccess.Models;

namespace GeekBlog.Contracts.ViewModels
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SortIndex { get; set; }
        public Image Image { get; set; }
        public string CreationDate { get; set; }
    }
}