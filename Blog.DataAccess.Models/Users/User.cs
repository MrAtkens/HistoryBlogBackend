using GeekBlog.DataAccess.Models.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using GeekBlog.DataAccess.Models.Enums;
using System;
using System.Net;
using System.Net.Mail;

namespace GeekBlog.DataAccess.Models
{
    public class User : Entity, IUser, IObserver
    {
        public string Email { get; set; }
        public bool Subscribe { get; set; }
        private string _passwordHash;

        public string PasswordHash
        {
            get => _passwordHash;
            set => _passwordHash = BCrypt.Net.BCrypt.HashPassword(value);
        }

        [Column(TypeName = "smallint")]
        public UserRole Role { get; set; }

        public bool PasswordCheck(string password)
        {
            if (BCrypt.Net.BCrypt.Verify(password, PasswordHash))
                return true;
            return false;
        }

        public void Update(Blog newBlog)
        {
          
        }
    }
}