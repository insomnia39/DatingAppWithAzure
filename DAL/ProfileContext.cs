using DatingApp.DAL.Model;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<MessageGroup> MessageGroup { get; set; }
        public DbSet<Message> Message { get; set; }

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

            modelBuilder.Entity<MessageGroup>(
                entity =>
                {
                    entity.ToContainer(nameof(MessageGroup));
                    entity.HasNoDiscriminator();
                    entity.HasKey(p => p.Id);
                    entity.HasPartitionKey(p => p.Partition);
                }
                );

            modelBuilder.Entity<Message>(
                entity =>
                {
                    entity.ToContainer(nameof(Message));
                    entity.HasNoDiscriminator();
                    entity.HasKey(p => p.Id);
                    entity.HasPartitionKey(p => p.Partition);
                }
                );
        }
    }
}
