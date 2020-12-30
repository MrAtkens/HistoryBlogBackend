﻿using BazarJok.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BazarJok.DataAccess.Domain
{
    public sealed class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var superAdmin = new Admin
            {
                CreationDate = DateTime.Now,
                Login = "dev",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123123"),
                Role = AdminRole.Developer
            };
            modelBuilder.Entity<Admin>().HasData(superAdmin);

            modelBuilder.Entity<Admin>()
                .HasIndex(a => a.Login)
                .IsUnique();
        }
    }
}
