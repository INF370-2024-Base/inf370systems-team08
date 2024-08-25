using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Models.Maintenance;
using EduProfileAPI.Repositories.Interfaces.Maintenance;
using Microsoft.EntityFrameworkCore;

namespace EduProfileAPI.Repositories.Implementation.Maintenance
{
    public class MaintenanceProcedureRepo: IMaintenanceProcedure
    {
        private readonly EduProfileDbContext _context;
        public MaintenanceProcedureRepo(EduProfileDbContext context)
        {
            _context = context;
        }

        public async Task<MaintenanceProcedure[]> GetAllProceduresAsync()
        {
            IQueryable<MaintenanceProcedure> query = _context.MaintenanceProcedure;
            return await query.ToArrayAsync();
        }

        public async Task<MaintenanceProcedure> GetProcedureAsync(Guid maintenanceProId)
        {
            IQueryable<MaintenanceProcedure> query = _context.MaintenanceProcedure.Where(c => c.MaintenanceProId == maintenanceProId);
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
