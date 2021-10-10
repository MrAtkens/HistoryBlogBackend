using GeekBlog.DataAccess.Models.Enums;

namespace GeekBlog.Contracts.Dtos
{
    public class UserClaimsDto
    {
        public string Email { get; set; }
        public UserRole Role { get; set; }

    }
}