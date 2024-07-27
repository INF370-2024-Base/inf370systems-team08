using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models;
using EduProfileAPI.Models.Maintenance;
using EduProfileAPI.Repositories.Interfaces.Maintenance;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation.Maintenance
{
    public class MaintenancePriorityRepo:IMaintenancePriority
    {
        private readonly EduProfileDbContext _context;
        public MaintenancePriorityRepo(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<MaintenancePriority[]> GetAllPriorityAsync()
        {
            IQueryable<MaintenancePriority> query = _context.MaintenancePriority;
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

