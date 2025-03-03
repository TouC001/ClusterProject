using System.ComponentModel.DataAnnotations;

namespace ClusterBackendAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public bool is_banned { get; set; }
        public string ban_reason { get; set; }
        public DateTime ban_until { get; set; }
        public string PasswordHash { get; set; }
        public List<UserRole> userRoles { get; set; }
    }
}
