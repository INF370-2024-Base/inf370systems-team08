using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace EduProfileAPI.PasswordValidator
{
    public class CustomPasswordValidator : IPasswordValidator<IdentityUser> 
    {
        public Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user, string password)
        {
            string pattern = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$";

            if (Regex.IsMatch(password, pattern))
            {
                return Task.FromResult(IdentityResult.Success);
            }
            else
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Description = "Password does not meet complexity requirements."
                }));
            }
        }
    }
}