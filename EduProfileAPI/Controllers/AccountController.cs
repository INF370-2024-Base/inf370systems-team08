using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EduProfileAPI.Models.User;
using EduProfileAPI.EmailService;
using Microsoft.Extensions.Configuration;
using EduProfileAPI.PasswordValidator;
using Org.BouncyCastle.Bcpg;
using Newtonsoft.Json.Linq;
using System.Web;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        public AccountController(UserManager<IdentityUser> userManager, IEmailService emailService, IConfiguration configuration)
        {
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized();
            }

            // Check if two-factor is enabled
            if (await _userManager.GetTwoFactorEnabledAsync(user))
            {
                var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

                await _emailService.SendEmailAsync("no-reply@yourdomain.com", user.Email,
                                                   "Your verification code",
                                                   $"Your code is: {token}");

                return Ok(new { Message = "Two factor authentication required.", userId = user.Id });
            }

            // Normal login when 2FA is not enabled
            var tokenString = GenerateJwtToken(user);
            return Ok(new { Token = tokenString });
        }


        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePassword model)
        {
            var user = await _userManager.FindByEmailAsync(model.userEmail);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.oldPassword))
            {
                var result = await _userManager.ChangePasswordAsync(user, model.oldPassword, model.newPassword);
                if (result.Succeeded)
                {
                    return Ok("Password updated successfully.");
                }
                return BadRequest(result.Errors);
            }
            return BadRequest("Invalid user or password");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassword model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest(new { message = "No user associated with email" });

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var frontendUrl = "http://localhost:4200/reset-password"; // Update this with your actual frontend URL
            var encodedToken = HttpUtility.UrlEncode(token);
            var callbackUrl = $"{frontendUrl}?token={encodedToken}&email={HttpUtility.UrlEncode(user.Email)}";

            // Send email with the reset password link
            await _emailService.SendEmailAsync("no-reply@yourdomain.com", user.Email,
                                    "Reset Your Password",
                                    $"Please reset your password by clicking <a href='{callbackUrl}'>here</a>.");

            return Ok(new { message = "Reset password link sent to email" });
        }



        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword model)
        {
            var user = await _userManager.FindByEmailAsync(model.email);
            if (user == null)
                return BadRequest("Invalid request");

            var result = await _userManager.ResetPasswordAsync(user, model.token, model.newPassword);
            if (result.Succeeded)
            {
                return Ok("Password has been reset successfully");
            }
            return BadRequest(result.Errors);
        }
         
        private string GenerateJwtToken(IdentityUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new IdentityUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email,
                TwoFactorEnabled = model.TwoFactorEnabled
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)

                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = string.Join("Please contact support, ", result.Errors.Select(x => x.Description)) });

            // Set IsActive to false
            await _userManager.AddClaimAsync(user, new Claim("IsActive", "false"));

            // Create a new entry in the User table with FirstName, LastName, and the FK to AspNetUsers
            var newUser = new User
            {
                UserId = Guid.NewGuid(), 
                FirstName = model.Name,  
                LastName = model.Surname, 
                AspNetUserId = user.Id,
                DisplayImage = null
            };

            // Add the new user to the database (Assume you have a DbContext injected as _dbContext)
            _dbContext.User.Add(newUser);
            await _dbContext.SaveChangesAsync();

            // Assign the role to the user by adding an entry to AspNetUserRoles
            var role = await _roleManager.FindByIdAsync(model.RoleId); 
            if (role != null)
            {
                await _userManager.AddToRoleAsync(user, role.Name);
            }

                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = string.Join(", ", result.Errors.Select(x => x.Description)) });


            return Ok(new { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost("verify-2fa")]
        public async Task<IActionResult> VerifyTwoFactorCode([FromBody] Verify2FA model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _userManager.VerifyTwoFactorTokenAsync(
                user,
                "Email", // The provider, should match the one used to generate the code
                model.Code);

            if (!result)
            {
                return BadRequest("Invalid 2FA code.");
            }

            // Generate JWT token upon successful 2FA verification
            var tokenString = GenerateJwtToken(user);
            return Ok(new { Token = tokenString });
        }
    }
}
