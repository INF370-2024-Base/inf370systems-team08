using Microsoft.Data.SqlClient;
using Quartz;
using System.Data;

namespace EduProfileAPI.AutoBackup
{
    public class DatabaseBackupJob:IJob
    {
        private readonly IConfiguration _configuration;
        private const string BackupFolderPath = @"C:\BackupFolder";  // Hardcoded backup path

        public DatabaseBackupJob(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var defaultConnectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqlConnection(defaultConnectionString))
                {
                    using (var command = new SqlCommand("sp_BackupDatabase", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@BackupPath", BackupFolderPath);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }

                Console.WriteLine("Backup completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Backup failed: {ex.Message}");
            }

            return Task.CompletedTask;
        }
    }
}
