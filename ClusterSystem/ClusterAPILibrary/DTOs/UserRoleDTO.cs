namespace ClusterAPILibrary.DTOs
{
    public class UserRoleDTO
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public RoleDTO RoleDTO { get; set; }

    }
}
