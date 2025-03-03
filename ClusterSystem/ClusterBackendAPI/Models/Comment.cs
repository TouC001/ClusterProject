namespace ClusterBackendAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Comments { get; set; }
        public int? ParentCommentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Comment> Replies { get; set; }

        // Vote counts for comments
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }

        public Post Post { get; set; }
        public User User { get; set; }
        public Comment ParentComment { get; set; }

        public ICollection<Vote> Votes { get; set; }
    }
}
