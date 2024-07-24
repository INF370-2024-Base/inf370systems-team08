namespace EduProfileAPI.ViewModels
{
    public class CreateEarlyReleaseVM
    {
        public Guid StudentId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime DateOfEarlyRelease { get; set; }
        public string ReasonForEarlyRelease { get; set; }
        public string SignerRelationship { get; set; }
        public string SignerName { get; set; }
        public string SignerIDNumber { get; set; }
    }
}
