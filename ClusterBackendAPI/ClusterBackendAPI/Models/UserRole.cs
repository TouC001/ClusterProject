namespace ClusterBackendAPI.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }

        // Navagation Properties to the Role and User Models.
        public Role Role { get; set; }
        public User User { get; set; }
    }
}
