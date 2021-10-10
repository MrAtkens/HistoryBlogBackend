using System.ComponentModel.DataAnnotations;

namespace GeekBlog.Contracts.Dtos
{
    public class SendMailDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Message { get; set; }
    }
}