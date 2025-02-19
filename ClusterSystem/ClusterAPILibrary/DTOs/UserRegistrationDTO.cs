using System.ComponentModel.DataAnnotations;

namespace ClusterAPILibrary.DTOs
{
    public class UserRegistrationDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public UserRoleDTO? userRoleDTO { get; set; }

        public UserRegistrationDTO()
        {
            userRoleDTO = new UserRoleDTO()
            {
                RoleId = 3,
                RoleDTO = new RoleDTO()
                {
                    Name = "Enjoryer"
                }
            };
        }
    }
}
