﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BazarJok.DataAccess.Models
{
    public class Image : Entity
    {
        public string ImageName { get; set; }
        public string Alt { get; set; }
        public string WebImagePath { get; set; }
    }
}
