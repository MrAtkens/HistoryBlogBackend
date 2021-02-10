﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BazarJok.DataAccess.Models
{
    public class Category : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int SortIndex { get; set; }
        public Image Image { get; set; }
    }
}
