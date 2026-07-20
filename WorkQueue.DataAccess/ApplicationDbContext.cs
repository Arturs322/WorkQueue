using Microsoft.EntityFrameworkCore;
using WorkQueue.Domain.Entities;

namespace WorkQueue.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Organization> Organizations => Set<Organization>();


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }
    }
}
