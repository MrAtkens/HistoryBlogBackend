﻿using BazarJok.DataAccess.Models.System;
using GeekBlog.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace GeekBlog.DataAccess.Domain
{
    public sealed class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            try
            {
                // It should throw exception when migrations are not available,
                // for example in a tests
                Database.EnsureCreated();
            }
            catch (InvalidOperationException e)
            {
                Database.EnsureCreated();
            }
                
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Quote> Quotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var superAdmin = new Admin
            {
                CreationDate = DateTime.Now,
                Login = "GeekHistoryGod",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("GeekNHistoRY#$#ff232"),
                Role = AdminRole.Admin
            };

            modelBuilder.Entity<Admin>().HasData(superAdmin);

            modelBuilder.Entity<Admin>()
                .HasIndex(a => a.Login)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
