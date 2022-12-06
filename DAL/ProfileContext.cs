using Microsoft.Azure.Cosmos;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Model.User>(
                entity =>
                {
                    entity.ToContainer(nameof(User));
                    entity.HasNoDiscriminator();
                    entity.HasKey(p => p.Id);
                    entity.HasPartitionKey(p => p.Partition);
                    entity.Property(p => p.Partition).ToJsonProperty("partition");
                    entity.Property(p => p.Id).ToJsonProperty("id");
                    entity.Property(p => p.Gender).ToJsonProperty("gender");
                    entity.Property(p => p.Username).ToJsonProperty("username");
                });
        }
    }
}
