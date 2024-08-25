using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.EmailService;
using EduProfileAPI.SmsService;
using EduProfileAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;
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
                    return Ok(new {message = "User IsActive status updated successfully." });
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


    }
}
