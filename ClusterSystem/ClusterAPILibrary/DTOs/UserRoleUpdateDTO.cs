namespace ClusterAPILibrary.DTOs
{
    public class UserRoleUpdateDTO
    {
        public int UserId { get; set; }
        public List<int> RoleIds { get; set; }
    }
}
