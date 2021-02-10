using System;

namespace BazarJok.Contracts.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string CreationDate { get; set; }
    }
}
