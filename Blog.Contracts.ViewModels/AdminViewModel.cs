using System;

namespace GeekBlog.Contracts.ViewModels
{
    public class AdminViewModel
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string CreationDate { get; set; }
    }
}