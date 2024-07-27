using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models.Maintenance;
using EduProfileAPI.Repositories.Interfaces.Maintenance;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation.Maintenance
{
    public class MaintenanceStatusRepo:IMaintenanceStatus
    {
        private readonly EduProfileDbContext _context;
        public MaintenanceStatusRepo(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<MaintenanceStatus[]> GetAllStatusAsync()
        {
            IQueryable<MaintenanceStatus> query = _context.MaintenanceStatus;
            return await query.ToArrayAsync();
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
