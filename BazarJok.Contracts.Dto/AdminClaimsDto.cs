using BazarJok.DataAccess.Models;

namespace BazarJok.Contracts.Dtos
{
    public class AdminClaimsDto
    {
        public string Login { get; set; }
        public AdminRole Role { get; set; }
    }
}
