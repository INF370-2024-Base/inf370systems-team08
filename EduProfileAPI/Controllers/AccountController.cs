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
using Microsoft.EntityFrameworkCore;
using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.SmsService;


namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EduProfileDbContext _dbContext;
        private readonly ISmsService _smsService;
        public AccountController(UserManager<IdentityUser> userManager, IEmailService emailService, ISmsService smsService, IConfiguration configuration, RoleManager<IdentityRole> roleManager, EduProfileDbContext dbContext)
        {
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _smsService = smsService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);


            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized(new { Message = "Incorrect Username or Password" });
            }

            // Check if the user is active
            var isActive = await _userManager.GetClaimsAsync(user);
            var isActiveClaim = isActive.FirstOrDefault(c => c.Type == "IsActive")?.Value;

            if (string.IsNullOrEmpty(isActiveClaim) || !bool.TryParse(isActiveClaim, out var isActiveFlag) || !isActiveFlag)
            {
                return Unauthorized(new { Message = "Your account is inactive. Please contact administrator." });
            }

            // Check if two-factor is enabled
            if (await _userManager.GetTwoFactorEnabledAsync(user))
            {
                var token = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider);
                var formattedPhoneNumber = "27" + user.PhoneNumber.Substring(1);

                await _smsService.SendSmsAsync(formattedPhoneNumber, $"Your verification code is: {token}");

                return Ok(new { Message = "Two factor authentication required.", userId = user.Id });
            }
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            // Normal login when 2FA is not enabled
            var tokenString = GenerateJwtToken(user);
            //check if they have userid
            var userTabel = _dbContext.User.FirstOrDefault(x => x.AspNetUserId == user.Id);
            if ( userTabel == null)
            {
                return Ok(new { Token = tokenString, Roles = role });
            }

            var employeeId = _dbContext.EmployeeUser.FirstOrDefault(x => x.UserId == userTabel.UserId).EmployeeId;
            if (employeeId == null)
            {
                return Ok(new { Token = tokenString, Roles = role, UserIds = userTabel.UserId });
            }

            return Ok(new { Token = tokenString, Roles = role, UserIds   = userTabel.UserId, EmployeeId = employeeId });
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
                    return Ok(new { Status = "Success", Message = "Password has been reset successfully" });
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
                return Ok(new { Status = "Success", Message="Password has been reset successfully" });
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

        //registering an account -- works conjunction with the saving of the information in the next endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            // Check if the user already exists
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User already exists!" });

            // Create the AspNetUser
            IdentityUser user = new IdentityUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email,
                TwoFactorEnabled = model.TwoFactorEnabled,
                PhoneNumber = model.PhoneNumber
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
            var role = await _roleManager.FindByIdAsync(model.RoleId); // Assuming model.RoleId is the selected role's ID
            if (role != null)
            {
                await _userManager.AddToRoleAsync(user, role.Name);
            }

            return Ok(new { Status = "Success", Message = "Registered successfuly, but please note your account is inactive untill approved by an admin." });
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
                TokenOptions.DefaultPhoneProvider,
                model.Code);

            if (!result)
            {
                return BadRequest("Invalid 2FA code.");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            // Generate JWT token upon successful 2FA verification
            var tokenString = GenerateJwtToken(user);
            var userTabel = _dbContext.User.FirstOrDefault(x => x.AspNetUserId == user.Id);
            if (userTabel == null)
            {
                return Ok(new { Token = tokenString, Roles = role });
            }


            var employeeId = _dbContext.EmployeeUser.FirstOrDefault(x => x.UserId == userTabel.UserId);
            if (employeeId == null)
            {
                return Ok(new { Token = tokenString, Roles = role, UserIds = userTabel.UserId });
            }

            return Ok(new { Token = tokenString, Roles = role, UserIds = userTabel.UserId, EmployeeId = employeeId });
        }



        //Role endpoint
        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return BadRequest(new { Status = "Error", Message = "Role name must be provided." });
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Role already exists!" });
            }

            var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (!roleResult.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = string.Join(", ", roleResult.Errors.Select(x => x.Description)) });
            }

            return Ok(new { Status = "Success", Message = "Role created successfully!" });
        }

        //retrieve all the roles
        [HttpGet("roles")]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles
                                    .Select(r => new Roles
                                    {
                                        Id = r.Id,
                                        Name = r.Name
                                    })
                                    .ToList();

            return Ok(roles);
        }

        
    }
}
