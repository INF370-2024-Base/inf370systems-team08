namespace EduProfileAPI.Models.User
{
    public class UpdatePassword
    {
        public string userEmail { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}
