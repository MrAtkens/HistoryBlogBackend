using System.ComponentModel.DataAnnotations;
using BazarJok.DataAccess.Models;

namespace BazarJok.Contracts.Dtos
{
    public class CategoryCreationDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int SortIndex { get; set; }
        [Required]
        public Image Image { get; set; }
    }
}