using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionManagementController : ControllerBase
    {
        private readonly EduProfileDbContext _dbContext;
        public PermissionManagementController( EduProfileDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("assign-permission/{userId}/{permissionId}")]
        public async Task<IActionResult> AssignPermissionToUser(Guid userId,int permissionId)
        {
            try
            {
                var userPermission = new UserPermissions
                {
                    Id = userId,
                    PermissionId = permissionId
                };

                _dbContext.UserPermissions.Add(userPermission);
                await _dbContext.SaveChangesAsync();

                return Ok("Permissions assigned");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error Contact Support: {ex.Message}");
            }
        }

        [HttpDelete("revoke-permission/{userId}/{permissionId}")]
        public async Task<IActionResult> RevokePermissionFromUser(Guid userId, int permissionId)
        {
            try
            {
                var userPermission = await _dbContext.UserPermissions
                    .FirstOrDefaultAsync(up => up.Id == userId && up.PermissionId == permissionId);

                if (userPermission != null)
                {
                    _dbContext.UserPermissions.Remove(userPermission);
                    await _dbContext.SaveChangesAsync();
                }

                return Ok("Permissions revoked");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error Contact Support: {ex.Message}");
            }
        }

        [HttpGet("get-permission")]
        public async Task<IActionResult> GetAllPermissionFor()
        {
            var permissions = await _dbContext.Permissions.ToListAsync();
            return Ok(permissions);
        }

        [HttpGet("get-userpermission")]
        public async Task<IActionResult> GetUserPermissionFor(Guid UserId)
        {
            var permissions = await _dbContext.Permissions
                .Where(p => _dbContext.UserPermissions
                       .Any(up => up.Id == UserId && up.PermissionId == p.PermissionId))
                .ToListAsync();
            return Ok(permissions);
        }
    }
}
