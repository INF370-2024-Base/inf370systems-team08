using EduProfileAPI.Controllers.Maintenance;
using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Models.Maintenance;
using EduProfileAPI.Repositories.Interfaces.Maintenance;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation.Maintenance
{
    public class MaintenanceRequestRepo:IMaintenanceRequest
    {
        private readonly EduProfileDbContext _context;
        public MaintenanceRequestRepo(EduProfileDbContext context)
        {
            _context = context;
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

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

