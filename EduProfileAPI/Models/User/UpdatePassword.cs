namespace EduProfileAPI.Models.User
{
    public class UpdatePassword
    {
        public string UserEmail { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
