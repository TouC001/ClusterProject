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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<Post>().ToTable("Posts");
            modelBuilder.Entity<Comment>().ToTable("Comments");
            modelBuilder.Entity<Vote>().ToTable("Votes");

            // User has many UserRoles, UserRole belongs to User
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.userRoles)
                .HasForeignKey(ur => ur.UserId);


            // UserRole has one Role, Role can be referenced by many UserRoles
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.userRoles)
                .HasForeignKey(ur => ur.RoleId);

            // Comment belongs to a User
            modelBuilder.Entity<Comment>()
                .HasOne(ur => ur.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);

            // Post has many comments, Comments belong to one Post
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId);

            // Vote belongs to a User (many votes per user)
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.User)
                .WithMany()
                .HasForeignKey(v => v.UserId);

            // Vote belongs to a Post (many votes per post)
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Post)
                .WithMany(p => p.Votes)
                .HasForeignKey(v => v.PostId)
                .OnDelete(DeleteBehavior.Cascade); // Ensure cascading delete

            // Vote belongs to a Comment (many votes per comment)
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Comment)
                .WithMany(c => c.Votes)
                .HasForeignKey(v => v.CommentId)
                .OnDelete(DeleteBehavior.Cascade); // Ensure cascading delete

            // Configure unique constraints to ensure no duplicate votes for same user on post/comment
            modelBuilder.Entity<Vote>()
                .HasIndex(v => new { v.UserId, v.PostId }).IsUnique();
            modelBuilder.Entity<Vote>()
                .HasIndex(v => new { v.UserId, v.CommentId }).IsUnique();
        }
        
    }
}
