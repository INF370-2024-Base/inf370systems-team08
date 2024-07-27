using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models.Maintenance;
using EduProfileAPI.Repositories.Interfaces.Maintenance;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation.Maintenance
{
    public class MaintenanceTypeRepo:IMaintenanceType
    {
        private readonly EduProfileDbContext _context;
        public MaintenanceTypeRepo(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<MaintenanceType[]> GetAllTypesAsync()
        {
            IQueryable<MaintenanceType> query = _context.MaintenanceType;
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
