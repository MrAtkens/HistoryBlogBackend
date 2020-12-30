using System;

namespace BazarJok.Contracts.ViewModels
{
    public class AdminViewModel
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public DateTime CreationDate { get; set; }
    }
}