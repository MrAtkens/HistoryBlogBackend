using System;
using System.Collections.Generic;
using System.Text;

namespace GeekBlog.DataAccess.Models.Interfaces
{
    public interface IUser {

        public bool PasswordCheck(string password);
    }
}
