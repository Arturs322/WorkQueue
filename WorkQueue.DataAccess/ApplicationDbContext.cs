using Microsoft.EntityFrameworkCore;
using WorkQueue.Domain.Entities;

namespace WorkQueue.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Organization> Organizations => Set<Organization>();
        public DbSet<WorkItem> WorkItems => Set<WorkItem>();
        public DbSet<Comment> Comments => Set<Comment>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkItem>()
                .Property(x => x.Version)
                .HasColumnName("xmin")
                .IsRowVersion();
            modelBuilder.Entity<WorkItem>()
                .HasOne(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<WorkItem>()
                .HasOne(x => x.AssigneeUser)
                .WithMany()
                .HasForeignKey(x => x.AssigneeUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
