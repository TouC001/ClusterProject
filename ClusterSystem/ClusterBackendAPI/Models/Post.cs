namespace ClusterBackendAPI.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        required public string Title { get; set; }
        public string? TextContent { get; set; }
        public string? ImagePath { get; set; }
        public string? ExternalLink { get; set; }
        public string CreatedAt { get; set; }

        // Vote counts for Posts
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }

        public User User { get; set; }

        // Navigation Property (One Post has Many Comments)
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<Vote> Votes { get; set; }
    }
}
