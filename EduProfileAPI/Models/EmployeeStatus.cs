using System;
using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class EmployeeStatus
    {
        [Key]
        public Guid EmployeeStatusId { get; set; }

        [StringLength(50)]
        public string Description { get; set; }
    }
}
