namespace ClusterAPILibrary.DTOs
{
    public class UserBannedDTO
    {
        public int Id { get; set; }
        public bool Is_Banned { get; set; }
        public string Ban_Reason { get; set; }
        public DateTime Ban_Until { get; set; }
        public int UserId { get; set; }

        public UserResponseDTO UserResponseDTO { get; set; }

    }
}
