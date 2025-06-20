﻿using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class Student
    {

        [Key]
        public Guid StudentId { get; set; }
        public Guid GradeId { get; set; }
        public Guid ClassId { get; set; }
        public Guid ParentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Boolean Gender { get; set; }
        public string Address { get; set; }
        public string AdmissionNo { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactRelationship { get; set; }
        public string EmergencyContactPhoneNum { get; set; }

        public ICollection<StudentSubject> StudentSubjects { get; set; }

    }
}
