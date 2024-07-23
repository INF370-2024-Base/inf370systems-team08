namespace EduProfileAPI.ViewModels

{
    public class StudentVM
    {
        public Guid GradeId { get; set; }
        public Guid ClassId { get; set; }
        public Guid ParentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public Boolean Gender { get; set; }
        public string Address { get; set; }
        public string AdmissionNo { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactRelationship { get; set; }
        public string EmergencyContactPhoneNum { get; set; }
    }
}
