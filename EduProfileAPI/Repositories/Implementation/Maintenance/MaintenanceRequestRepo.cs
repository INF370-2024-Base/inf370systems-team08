using EduProfileAPI.Controllers.Maintenance;
using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Models.Maintenance;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.Repositories.Interfaces.Maintenance;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EduProfileAPI.Repositories.Implementation.Maintenance
{
    public class MaintenanceRequestRepo:IMaintenanceRequest
    {
        private readonly EduProfileDbContext _context;
        private readonly IAuditTrail _auditTrailRepository; // Inject the audit trail repository

        public MaintenanceRequestRepo(EduProfileDbContext context, IAuditTrail auditTrailRepository)
        {
            _context = context;
            _auditTrailRepository = auditTrailRepository; // Assign the injected audit trail repository

        }

        // Call the stored procedure to retrieve all maintenance requests
        public async Task<List<MaintenanceRequest>> GetAllRequestsAsync()
        {
            return await _context.MaintenanceRequest
                .FromSqlRaw("EXEC GetAllRequests")
                .ToListAsync();
        }

        public async Task<MaintenanceRequest> GetRequestAsync(Guid maintenanceReqId)
        {
            IQueryable<MaintenanceRequest> query = _context.MaintenanceRequest.Where(c => c.MaintenanceReqId == maintenanceReqId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task AddRequestAsync(MaintenanceRequest request, Guid userId)
        {
            _context.Add(request);

            // Log the audit trail for the creation
            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "CREATE",
                EntityName = "MaintenanceRequest",
                AffectedEntityID = request.MaintenanceReqId,
                NewValue = JsonConvert.SerializeObject(request), // Log the new request data as JSON
                TimeStamp = DateTime.UtcNow
            };

            await _auditTrailRepository.AddAuditTrailAsync(auditTrail);
        }

        // Update an existing maintenance request and log the action
        public async Task UpdateRequestAsync(MaintenanceRequest updatedRequest, MaintenanceRequest oldRequest, Guid userId)
        {
            _context.Entry(oldRequest).CurrentValues.SetValues(updatedRequest);

            // Log the audit trail for the update
            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "UPDATE",
                EntityName = "MaintenanceRequest",
                AffectedEntityID = updatedRequest.MaintenanceReqId,
                OldValue = JsonConvert.SerializeObject(oldRequest),  // Log the old data
                NewValue = JsonConvert.SerializeObject(updatedRequest), // Log the new data
                TimeStamp = DateTime.UtcNow
            };

            await _auditTrailRepository.AddAuditTrailAsync(auditTrail);
        }

        // Delete a maintenance request and log the action
        public async Task DeleteRequestAsync(MaintenanceRequest request, Guid userId)
        {
            _context.MaintenanceRequest.Remove(request);

            // Log the audit trail for the deletion
            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "DELETE",
                EntityName = "MaintenanceRequest",
                AffectedEntityID = request.MaintenanceReqId,
                OldValue = JsonConvert.SerializeObject(request), // Log the old request data
                TimeStamp = DateTime.UtcNow
            };

            await _auditTrailRepository.AddAuditTrailAsync(auditTrail);
        }


        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

