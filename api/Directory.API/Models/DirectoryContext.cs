using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Directory.API.Models;

namespace Directory.API.Models
{
    public class DirectoryContext : DbContext
    {
        public DirectoryContext(DbContextOptions<DirectoryContext> options) : base(options)
        {
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Family> Families { get; set; }

        public DbSet<FamilyPerson> FamilyPerson { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FamilyPerson>()
                .HasKey(fp => new {fp.PersonId, fp.FamilyId});

            modelBuilder.Entity<Family>()
                .HasMany(f => f.FamilyMembers)
                .WithOne();

            modelBuilder.Entity<Person>()
                .HasMany(fp => fp.FamilyRoles)
                .WithOne();
        }
    }
}