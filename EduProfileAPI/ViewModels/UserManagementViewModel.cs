﻿namespace EduProfileAPI.ViewModels
{
    public class UserManagementViewModel
    {
        public string UserId { get; set; }
        public string AspNetUserId { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
    }
}
