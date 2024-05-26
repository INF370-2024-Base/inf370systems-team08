namespace EduProfileAPI.Models.User
{
    public class ResetPassword
    {
        public string email { get; set; }
        public string token { get; set; }
        public string newPassword { get; set; }
    }
}
