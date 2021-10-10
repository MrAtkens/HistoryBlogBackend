using System;
using GeekBlog.DataAccess.Models.Enums;
using Microsoft.AspNetCore.Authorization;


namespace GeekBlog.Contracts.Attributes
{
    public sealed class AdminAuthorizedAttribute : AuthorizeAttribute
    {
        
        public AdminAuthorizedAttribute(params AdminRole[] roles) : base()
        {
            Roles = String.Join(",", roles);
        }

    }

}
