using BazarJok.DataAccess.Models;

namespace BazarJok.Contracts.Dtos
{
    public class CategoryCreationDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int SortIndex { get; set; }
        public Image Image { get; set; }
    }
}