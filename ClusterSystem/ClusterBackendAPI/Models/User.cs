using System.ComponentModel.DataAnnotations;

namespace ClusterBackendAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public List<UserRole> userRoles { get; set; }
    }
}
