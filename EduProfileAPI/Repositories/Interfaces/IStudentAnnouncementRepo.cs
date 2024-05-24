namespace EduProfileAPI.Repositories.Interfaces
{
    public interface IStudentAnnouncementRepo
    {
        void Add<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();


    }
}
