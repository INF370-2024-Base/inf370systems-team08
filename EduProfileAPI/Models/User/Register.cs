﻿namespace EduProfileAPI.Models.User
{
    public class Register
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string  RoleId { get; set; }
    }
}
