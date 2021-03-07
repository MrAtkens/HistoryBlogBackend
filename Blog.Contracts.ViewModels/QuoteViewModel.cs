﻿using System;
using BazarJok.DataAccess.Models;

namespace BazarJok.Contracts.ViewModels
{
    public class QuoteViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } 
        public string Description { get; set; } 
        public string Date { get; set; } 
        public Image Image { get; set; }
        public string CreationDate { get; set; }
    }
}