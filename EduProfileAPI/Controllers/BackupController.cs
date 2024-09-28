using EduProfileAPI.AutoBackup;
using EduProfileAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Quartz.Impl;
using Quartz;
using System.Data;

namespace EduProfileAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackupController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private const string BackupFolderPath = @"C:\BackupFolder";
        private readonly ISchedulerFactory _schedulerFactory;

        public BackupController(IConfiguration configuration, ISchedulerFactory schedulerFactory)
        {
            _configuration = configuration;
            _schedulerFactory = schedulerFactory;
        }

        [HttpPost("manual-backup")]
        public IActionResult ManualBackup()
        {
            try
            {
                
                var defaultConnectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqlConnection(defaultConnectionString))
                {
                    using (var command = new SqlCommand("sp_BackupDatabase", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@BackupPath", BackupFolderPath);  // Use hardcoded path

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }

                return Ok("Backup completed successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Backup failed: {ex.Message}");
            }
        }

        [HttpPost("restore-latest")]
        public IActionResult RestoreLatestBackup()
        {
            try
            {
                var masterConnectionString = _configuration.GetConnectionString("MasterConnection");

                using (var connection = new SqlConnection(masterConnectionString))
                {
                    using (var command = new SqlCommand("sp_RestoreLatestBackup", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }

                return Ok("Restore completed successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Restore failed: {ex.Message}");
            }
        }

        [HttpPost("configure-schedule")]
        public async Task<IActionResult> ConfigureBackupSchedule([FromBody] BackupSchedule scheduleModel)
        {
            try
            {
                // Save the scheduleModel data into your database.
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var query = @"INSERT INTO BackupSchedule (Frequency, BackupTime, DaysOfWeek) 
                          VALUES (@Frequency, @BackupTime, @DaysOfWeek)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Frequency", scheduleModel.Frequency);
                        command.Parameters.AddWithValue("@BackupTime", scheduleModel.BackupTime);
                        command.Parameters.AddWithValue("@DaysOfWeek", scheduleModel.DaysOfWeek ?? (object)DBNull.Value);

                        connection.Open();
                        await command.ExecuteNonQueryAsync();  // Use async for better performance
                        connection.Close();
                    }
                }

                // Trigger Quartz.NET scheduling logic
                var scheduler = await StdSchedulerFactory.GetDefaultScheduler();

                // Define the job
                var jobKey = new JobKey("DatabaseBackupJob");

                var currentTrigger = await scheduler.GetTrigger(new TriggerKey("DatabaseBackupTrigger"));
                if (currentTrigger != null)
                {
                    await scheduler.UnscheduleJob(new TriggerKey("DatabaseBackupTrigger"));
                }

                var jobExists = await scheduler.CheckExists(jobKey);
                if (!jobExists)
                {
                    var job = JobBuilder.Create<DatabaseBackupJob>()
                                        .WithIdentity(jobKey)
                                        .Build();

                    var cronExpression = ConvertScheduleToCron(scheduleModel);
                    var trigger = TriggerBuilder.Create()
                                                .WithIdentity("DatabaseBackupTrigger")
                                                .WithCronSchedule(cronExpression)  
                                                .ForJob(jobKey)
                                                .Build();

                    await scheduler.ScheduleJob(job, trigger);

                    var nextFireTime = trigger.GetNextFireTimeUtc();
                    Console.WriteLine($"Next scheduled fire time: {nextFireTime}");


                    var now = DateTime.Now;
                    var timeToTrigger = scheduleModel.BackupTime - now.TimeOfDay;
                    await Task.Delay(timeToTrigger);
                    var result = await TriggerBackupManuallyAsync();
                    if (result)
                    {
                        return Ok("Backup schedule configured successfully.");
                    }
                    else
                    {
                        return BadRequest("Failed to trigger backup job manually.");
                    }
                        


                }
                else
                {
                    var cronExpression = ConvertScheduleToCron(scheduleModel);
                    var trigger = TriggerBuilder.Create()
                                                .WithIdentity("DatabaseBackupTrigger")
                                                .WithCronSchedule(cronExpression)  
                                                .ForJob(jobKey)
                                                .Build();

                    await scheduler.RescheduleJob(new TriggerKey("DatabaseBackupTrigger"), trigger);

                    var nextFireTime = trigger.GetNextFireTimeUtc();
                    Console.WriteLine($"Next scheduled fire time: {nextFireTime}");

                }

                return Ok("Backup schedule configured successfully.");



            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to configure backup schedule: {ex.Message}");
            }
        }

        [HttpGet("current-schedule")]
        public async Task<IActionResult> GetCurrentBackupSchedule()
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var query = "SELECT TOP 1 Frequency, BackupTime, DaysOfWeek FROM BackupSchedule ORDER BY CreatedAt DESC";

                    using (var command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                var schedule = new BackupSchedule
                                {
                                    Frequency = reader["Frequency"].ToString(),
                                    BackupTime = TimeSpan.Parse(reader["BackupTime"].ToString()),
                                    DaysOfWeek = reader["DaysOfWeek"] != DBNull.Value ? reader["DaysOfWeek"].ToString() : null
                                };

                                return Ok(schedule);
                            }
                            else
                            {
                                return NotFound("No backup schedule found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving backup schedule: {ex.Message}");
            }
        }

        private async Task<bool> TriggerBackupManuallyAsync()
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKey = new JobKey("DatabaseBackupJob");

            if (await scheduler.CheckExists(jobKey))
            {
                await scheduler.TriggerJob(jobKey);  // Manually trigger the job
                return true;
            }
            else
            {
                return false;  // Job not found
            }
        }


        private string ConvertScheduleToCron(BackupSchedule scheduleModel)
        {
            var adjustedBackupTime = DateTime.Today.Add(scheduleModel.BackupTime);

            if (scheduleModel.Frequency == "Daily")
            {
                return $"0 {scheduleModel.BackupTime.Minutes} {scheduleModel.BackupTime.Hours} ? * * *";
            }
            else if (scheduleModel.Frequency == "Weekly")
            {
                var daysOfWeek = scheduleModel.DaysOfWeek; // e.g., "Mon,Wed,Fri"
                return $"{scheduleModel.BackupTime.Minutes} {scheduleModel.BackupTime.Hours} ? * {daysOfWeek} *";
            }
            else if (scheduleModel.Frequency == "Monthly")
            {
                return $"{scheduleModel.BackupTime.Minutes} {scheduleModel.BackupTime.Hours} 1 * *?";
            }

            return "0 0 2 ? * * *";
        }


    }

}

