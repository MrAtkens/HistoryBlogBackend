using GeekBlog.DataAccess.Models.Enums;
using GeekBlog.DataAccess.Models.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekBlog.DataAccess.Models
{
    public class Admin : Entity, IUser
    {
        public string Login { get; set; }
        private string _passwordHash;

        public string PasswordHash
        {
            get => _passwordHash;
            set => _passwordHash = BCrypt.Net.BCrypt.HashPassword(value);
        }

        [Column(TypeName = "smallint")]
        public AdminRole Role { get; set; }

        public bool PasswordCheck(string password)
        {
            if (BCrypt.Net.BCrypt.Verify(password, PasswordHash))
                return true;
            return false;
        }
    }
}
