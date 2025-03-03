using System.ComponentModel.DataAnnotations;

namespace ClusterAPILibrary.DTOs
{
    public class UserUpdateDTO
    {
        public string Username { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
