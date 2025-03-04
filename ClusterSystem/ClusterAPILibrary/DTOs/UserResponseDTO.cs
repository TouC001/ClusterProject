using System.ComponentModel.DataAnnotations;

namespace ClusterAPILibrary.DTOs
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public bool Is_banned { get; set; }
        public List<UserRoleDTO> userRoleDTOs { get; set; }
    }
}
