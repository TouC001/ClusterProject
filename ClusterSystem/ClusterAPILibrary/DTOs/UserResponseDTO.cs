namespace ClusterAPILibrary.DTOs
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public List<UserRoleDTO> userRoleDTOs { get; set; }
    }
}
