using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.EmailService;
using EduProfileAPI.Models;
using EduProfileAPI.SmsService;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagmentController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EduProfileDbContext _dbContext;
        private readonly ISmsService _smsService;
        public UserManagmentController(UserManager<IdentityUser> userManager, IEmailService emailService, ISmsService smsService, IConfiguration configuration, RoleManager<IdentityRole> roleManager, EduProfileDbContext dbContext)
        {
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _smsService = smsService;
        }

        [HttpGet("get-users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                // Get all users from the User table, ordered by the latest addition in AspNetUsers
                var users = await _dbContext.User
                    .ToListAsync();

                users.Reverse();

                var userManagementList = new List<UserManagementViewModel>();

                foreach (var user in users)
                {
                    // Get the corresponding IdentityUser from the AspNetUsers table using the AspNetUserId
                    var identityUser = await _userManager.FindByIdAsync(user.AspNetUserId);

                    var roles = await _userManager.GetRolesAsync(identityUser);
                    var isActiveClaim = await _userManager.GetClaimsAsync(identityUser);
                    var isActive = isActiveClaim.FirstOrDefault(c => c.Type == "IsActive")?.Value == "true";

                    var userViewModel = new UserManagementViewModel
                    {
                        UserId = user.UserId.ToString(),
                        AspNetUserId = identityUser.Id,
                        FirstName = user.FirstName ?? string.Empty,
                        LastName = user.LastName ?? string.Empty,
                        Email = identityUser.Email ?? string.Empty,
                        PhoneNumber = identityUser.PhoneNumber ?? string.Empty,
                        Role = roles.FirstOrDefault() ?? "No Role Assigned",
                        IsActive = isActive
                    };

                    userManagementList.Add(userViewModel);
                }

                return Ok(userManagementList);
            }
            catch (SqlNullValueException ex)
            {
                // Log the exception (optional)
                // Handle the exception appropriately
                return StatusCode(500, $"{ex}   A database null value was encountered. Please check the data and try again.");
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                // Handle any other exceptions
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("update-user-isactive")]
        public async Task<IActionResult> UpdateUserIsActive([FromBody] UpdateUserIsActiveViewModel model)
        {
            try
            {
                // Find the IdentityUser using the AspNetUserId
                var identityUser = await _userManager.FindByIdAsync(model.AspNetUserId);

                if (identityUser == null)
                {
                    return NotFound($"User with Id {model.AspNetUserId} not found.");
                }

                // Get the IsActive claim
                var isActiveClaim = (await _userManager.GetClaimsAsync(identityUser))
                                    .FirstOrDefault(c => c.Type == "IsActive");

                if (isActiveClaim != null)
                {
                    // Remove the existing claim
                    await _userManager.RemoveClaimAsync(identityUser, isActiveClaim);
                }

                // Add the new IsActive claim
                var newClaim = new Claim("IsActive", model.IsActive.ToString().ToLower());
                var result = await _userManager.AddClaimAsync(identityUser, newClaim);

                if (result.Succeeded)
                {
                    var emailSubject = model.IsActive ? "Access Enabled" : "Access Disabled";
                    var emailBody = model.IsActive
                        ? "Your access to EduProfile has been enabled. You can now log in to your account.😀"
                        : "Your access to EduProfile has been disabled. You will not be able to log in until your access is re-enabled. Contact an administrator at the school";

                    await _emailService.SendEmailAsync("no-reply@yourdomain.com", identityUser.Email, emailSubject, emailBody);
                    return Ok(new { message = "User IsActive status updated successfully." });
                }
                else
                {
                    return BadRequest("Failed to update user IsActive status.");
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An error occurred while processing your request.");

            }
        }

        [HttpGet("get-user-profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            try
            {
                // Extract the JWT token from the Authorization header
                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    return Unauthorized("Invalid token.");
                }

                var token = authHeader.Substring("Bearer ".Length).Trim();

                // Validate the token and extract the claims
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized("User ID not found in token.");
                }

                // Find the user in AspNetUsers
                var identityUser = await _userManager.FindByIdAsync(userIdClaim);
                if (identityUser == null)
                {
                    return NotFound("User not found.");
                }

                // Find the user in the User table
                var user = await _dbContext.User
                    .FirstOrDefaultAsync(u => u.AspNetUserId == identityUser.Id);

                if (user == null)
                {
                    return NotFound("User profile not found.");
                }

                // Get the user's role
                var roles = await _userManager.GetRolesAsync(identityUser);
                var role = roles.FirstOrDefault() ?? "No Role Assigned";

                // Prepare the ViewModel
                var userProfile = new UserProfileViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = identityUser.Email,
                    PhoneNumber = identityUser.PhoneNumber,
                    Role = role,
                    DisplayImage = user.DisplayImage ?? Array.Empty<byte>()
                };

                return Ok(userProfile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }

        }

        [HttpPost("update-profile-picture")]
        public async Task<IActionResult> UpdateProfilePicture([FromBody] string base64ImageData)
        {
            try
            {
                // Extract the JWT token from the Authorization header
                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    return Unauthorized("Invalid token.");
                }

                var token = authHeader.Substring("Bearer ".Length).Trim();

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

                // Find the user in AspNetUsers
                var identityUser = await _userManager.FindByIdAsync(userIdClaim);
                if (identityUser == null)
                {
                    return NotFound("User not found.");
                }

                // Find the user in the User table
                var user = await _dbContext.User
                    .FirstOrDefaultAsync(u => u.AspNetUserId == identityUser.Id);

                if (user == null)
                {
                    return NotFound("User profile not found.");
                }

                // Validate the Base64 string
                if (string.IsNullOrEmpty(base64ImageData))
                {
                    return BadRequest("No image data received or image data is empty.");
                }

                // Convert the Base64 string to a byte array
                byte[] imageData = Convert.FromBase64String(base64ImageData);

                // Update the user's DisplayImage
                user.DisplayImage = imageData;

                // Save changes to the database
                _dbContext.User.Update(user);
                await _dbContext.SaveChangesAsync();

                return Ok(new { message = "Profile picture updated successfully." });
            }
            catch (FormatException)
            {
                return BadRequest("Invalid Base64 string.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("update-user-role")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleViewModel model)
        {
            try
            {
                // Find the IdentityUser using the AspNetUserId
                var identityUser = await _userManager.FindByIdAsync(model.AspNetUserId);

                if (identityUser == null)
                {
                    return NotFound($"User with Id {model.AspNetUserId} not found.");
                }

                // Get the current roles of the user
                var currentRoles = await _userManager.GetRolesAsync(identityUser);

                // Remove the user from all current roles
                var removeResult = await _userManager.RemoveFromRolesAsync(identityUser, currentRoles);
                if (!removeResult.Succeeded)
                {
                    return BadRequest("Failed to remove user from current roles.");
                }

                // Add the user to the new role
                var addResult = await _userManager.AddToRoleAsync(identityUser, model.NewRole);
                if (addResult.Succeeded)
                {
                    return Ok(new { message = "User role updated successfully." });
                }
                else
                {
                    return BadRequest("Failed to add user to the new role.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("assign-user-to-employee")]
        public async Task<IActionResult> AssignUserToEmployee([FromBody] AssignUserToEmployeeViewModel model)
        {
            try
            {
                var employeeUser = await _dbContext.EmployeeUser.FirstOrDefaultAsync(eu => eu.UserId == model.UserId);
                if (employeeUser != null)
                {
                    return BadRequest("User is already assigned to an employee.");
                }
                var newEmployeeUser = new EmployeeUser
                {
                    UserId = model.UserId,
                    EmployeeId = model.EmployeeId,
                    Description = model.Description
                };

                await _dbContext.EmployeeUser.AddAsync(newEmployeeUser);
                await _dbContext.SaveChangesAsync();

                return Ok(new { message = "User assigned to employee successfully." });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing your request. {ex}");
            }
        }

        [HttpPost("assign-user-to-student")]
        public async Task<IActionResult> AssignUserToStudent([FromBody] AssignUserToStudentViewModel model)
        {
            try
            {
                var studentUser = await _dbContext.StudentUser.FirstOrDefaultAsync(su => su.UserId == model.UserId);
                if (studentUser != null)
                {
                    return BadRequest("User is already assigned to a student.");
                }

                var newStudentUser = new StudentUser
                {
                    UserId = model.UserId,
                    StudentId = model.StudentId,
                    Description = model.Description
                };

                await _dbContext.StudentUser.AddAsync(newStudentUser);
                await _dbContext.SaveChangesAsync();

                return Ok(new { message = "User assigned to student successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing your request.{ex}");
            }
        }

    }
}
