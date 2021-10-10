using GeekBlog.DataAccess.Models.Enums;

namespace GeekBlog.Contracts.Dtos
{
    public class UserCreationDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}