using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class StudentAttendance
    {
        [Key]
        public Guid StudentAttendanceId { get; set; }
        public Guid StudentId { get; set; }
        public Guid ClassId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public Guid AttendanceStatusId { get; set; }

    }
}

