using System;
using System.Collections.Generic;
using System.Text;

namespace BazarJok.DataAccess.Models
{
    public class Category : Entity
    {
        public string Name { get; set; }
        public int SortIndex { get; set; }
    }
}
