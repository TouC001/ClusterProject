namespace ClusterAPILibrary.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } // Include username for display
        public int PostId { get; set; }
        public string Text { get; set; }
        public int? ParentCommentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CommentDTO> Replies { get; set; } // Recursive DTO for nested comments
    }
}
