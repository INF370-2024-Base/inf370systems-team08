using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models.User
{
    public class User
    {
        
        public Guid UserId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
