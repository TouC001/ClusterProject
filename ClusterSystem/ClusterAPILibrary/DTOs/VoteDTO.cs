namespace ClusterAPILibrary.DTOs
{
    public class VoteDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? PostId { get; set; }  // Include PostId if voting on a post
        public int? CommentId { get; set; }  // Include CommentId if voting on a comment
        public int VoteType { get; set; }  // 1 for upvote, -1 for downvote
    }
}
