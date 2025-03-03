using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClusterBackendAPI.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public int VoteType { get; set; }  // 1 for upvote, -1 for downvote

        public User User { get; set; }
        public Post Post { get; set; }
        public Comment Comment { get; set; }

        // Configure unique constraints to ensure no duplicate votes for same user on post/comment
        public static void Configure(EntityTypeBuilder<Vote> builder)
        {
            // Index for PostId to speed up queries for votes on posts
            builder.HasIndex(v => v.PostId);

            // Index for CommentId to speed up queries for votes on comments
            builder.HasIndex(v => v.CommentId);

            builder.HasIndex(v => new { v.UserId, v.PostId }).IsUnique();
            builder.HasIndex(v => new { v.UserId, v.CommentId }).IsUnique();
        }
    }
}
