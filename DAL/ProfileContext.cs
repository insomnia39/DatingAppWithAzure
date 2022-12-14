using DatingApp.DAL.Model;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;

namespace DatingApp.DAL
{
    public static class Profile
    {
        public static readonly string _containerName = nameof(Profile);
    }

    public class ProfileContext : DbContext
    {
        public ProfileContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Model.User> User { get; set; }
        public DbSet<Photo> Photo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Model.User>(
                entity =>
                {
                    entity.ToContainer(nameof(User));
                    entity.HasNoDiscriminator();
                    entity.HasKey(p => p.Id);
                    entity.HasPartitionKey(p => p.Partition);
                });

            modelBuilder.Entity<Photo>(
                entity =>
                {
                    entity.ToContainer(nameof(Photo));
                    entity.HasNoDiscriminator();
                    entity.HasKey(p => p.Id);
                    entity.HasPartitionKey(p => p.Partition);
                });
        }
    }
}
