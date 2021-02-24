namespace BazarJok.Contracts.ViewModels
{
    public class AdminBlogViewModel : BlogViewModel
    {
        public int ViewCount { get; set; }
        public bool IsFeatured { get; set; }
    }
}