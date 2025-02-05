using ClusterBackendAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClusterBackendAPI.DataContext
{
    public class ClusterDbContext : DbContext 
    {
        public ClusterDbContext(DbContextOptions<ClusterDbContext> options) : base(options) { }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Vote> Votes { get; set; }

    }
}
