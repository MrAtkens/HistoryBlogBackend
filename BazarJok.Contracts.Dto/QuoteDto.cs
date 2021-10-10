using System.ComponentModel.DataAnnotations;

namespace GeekBlog.Contracts.Dtos
{
    public class QuoteDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public ImageDto ImageDto { get; set; }
    }
}