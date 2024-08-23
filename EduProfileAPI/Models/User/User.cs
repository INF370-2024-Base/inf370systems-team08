namespace EduProfileAPI.Models.User
{
    public class User
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[]? DisplayImage { get; set; }
        public string AspNetUserId { get; set; }
    }
}
