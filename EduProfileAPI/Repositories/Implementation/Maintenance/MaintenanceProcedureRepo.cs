using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Models.Maintenance;
using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.Repositories.Interfaces.Maintenance;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EduProfileAPI.Repositories.Implementation.Maintenance
{
    public class MaintenanceProcedureRepo: IMaintenanceProcedure
    {
        private readonly EduProfileDbContext _context;
        private readonly IAuditTrail _auditTrailRepository;
        public MaintenanceProcedureRepo(EduProfileDbContext context,IAuditTrail auditTrailRepository)
        {
            _context = context;
            _auditTrailRepository = auditTrailRepository;


        }

        // Call the stored procedure to retrieve all maintenance procedures
        public async Task<List<MaintenanceProcedure>> GetAllProceduresAsync()
        {
            return await _context.MaintenanceProcedure
                .FromSqlRaw("EXEC GetAllProcedures")
                .ToListAsync();
        }

        public async Task<MaintenanceProcedure> GetProcedureAsync(Guid maintenanceProId)
        {
            IQueryable<MaintenanceProcedure> query = _context.MaintenanceProcedure.Where(c => c.MaintenanceProId == maintenanceProId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task AddProcedureAsync(MaintenanceProcedure procedure, Guid userId)
        {
            _context.Add(procedure);

            // Log the creation in the audit trail
            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "CREATE",
                EntityName = "MaintenanceProcedure",
                AffectedEntityID = procedure.MaintenanceProId,
                NewValue = JsonConvert.SerializeObject(procedure),
                TimeStamp = DateTime.UtcNow
            };
            await _auditTrailRepository.AddAuditTrailAsync(auditTrail);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateProcedureAsync(MaintenanceProcedure updatedProcedure, MaintenanceProcedure oldProcedure, Guid userId)
        {
            _context.Entry(oldProcedure).CurrentValues.SetValues(updatedProcedure);

            // Log the update in the audit trail
            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "UPDATE",
                EntityName = "MaintenanceProcedure",
                AffectedEntityID = updatedProcedure.MaintenanceProId,
                OldValue = JsonConvert.SerializeObject(oldProcedure),
                NewValue = JsonConvert.SerializeObject(updatedProcedure),
                TimeStamp = DateTime.UtcNow
            };
            await _auditTrailRepository.AddAuditTrailAsync(auditTrail);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteProcedureAsync(MaintenanceProcedure procedure, Guid userId)
        {
            _context.MaintenanceProcedure.Remove(procedure);

            // Log the deletion in the audit trail
            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "DELETE",
                EntityName = "MaintenanceProcedure",
                AffectedEntityID = procedure.MaintenanceProId,
                OldValue = JsonConvert.SerializeObject(procedure),
                TimeStamp = DateTime.UtcNow
            };
            await _auditTrailRepository.AddAuditTrailAsync(auditTrail);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
