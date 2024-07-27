using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.Models
{
    public class Parent
    {
        [Key]
        public Guid ParentId { get; set; }
        public string Parent1Name { get; set; }
        public bool Parent1MaritalStatus { get; set; }
        public string Parent1Occupation { get; set; }
        public string Parent1PhysicalAddress { get; set; }
        public string Parent1PostalAddress { get; set; }
        public string Parent1HomePhone { get; set; }
        public string Parent1WorkPhone { get; set; }
        public string Parent1CellPhone { get; set; }
        public string Parent2Name { get; set; }
        public bool Parent2MaritalStatus { get; set; }
        public string Parent2Occupation { get; set; }
        public string Parent2PhysicalAddress { get; set; }
        public string Parent2PostalAddress { get; set; }
        public string Parent2HomePhone { get; set; }
        public string Parent2WorkPhone { get; set; }
        public string Parent2CellPhone { get; set; }
    }
}
