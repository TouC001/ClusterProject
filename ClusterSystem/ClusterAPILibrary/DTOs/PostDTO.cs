namespace ClusterAPILibrary.DTOs
{
    public class PostDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        required public string Title { get; set; }
        public string? TextContent { get; set; }
        public string? ImagePath { get; set; }
        public string? ExternalLink { get; set; }
        public string CreatedAt { get; set; }

        public UserDTO User { get; set; }
    }

    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }
}
