using System.ComponentModel.DataAnnotations.Schema;

namespace BazarJok.DataAccess.Models
{
    public class Admin : Entity
    {
        public string Login { get; set; }
        public string PasswordHash { get; set; }

        [Column(TypeName = "tinyint")]
        public AdminRole Role { get; set; }
    }
}
