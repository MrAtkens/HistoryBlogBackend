using System;

namespace BazarJok.DataAccess.Models
{
    public class Quote : Entity
    {
        public string FullName { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public Image Image { get; set; }
    }
}