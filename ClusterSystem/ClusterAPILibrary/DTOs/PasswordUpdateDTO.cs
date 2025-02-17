namespace ClusterAPILibrary.DTOs
{
    public class PasswordUpdateDTO
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
