using GeekBlog.DataAccess.Models.Enums;

namespace GeekBlog.Contracts.Dtos
{
    public class AdminClaimsDto
    {
        public string Login { get; set; }
        public AdminRole Role { get; set; }
    }
}
