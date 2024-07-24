using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.ViewModels
{
    public class CreateDisciplinaryVM
    {
        public Guid DisciplinaryTypeId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid StudentId { get; set; }
       
        public string Reason { get; set; }

        public bool? ParentContacted { get; set; }

        public string DisciplinaryDuration { get; set; }

        public DateTime? IssueDate { get; set; }
    }
}
