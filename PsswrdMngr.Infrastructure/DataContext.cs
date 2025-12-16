using Microsoft.EntityFrameworkCore;
using PsswrdMngr.Domain;

namespace PsswrdMngr.Infrastructure
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<CredentialContainer> CredentialContainers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(x => x.Name)
                    .IsUnique();

                entity.Property(x => x.PasswordHash)
                    .IsRequired();
            });

            modelBuilder.Entity<CredentialContainer>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.ContainerHash).IsRequired();
                entity.Property(x => x.ContainerString).IsRequired();
            });
        }
    }
}
