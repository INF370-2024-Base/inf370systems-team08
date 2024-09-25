namespace EduProfileAPI.ViewModels
{
    public class StudentAttendanceViewModel
    {
        public Guid StudentId { get; set; }
        public Guid ClassId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public Guid AttendanceStatusId { get; set; }

    }
}
