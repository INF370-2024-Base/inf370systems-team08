using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using EduProfileAPI.WhatsApp;

namespace EduProfileAPI.Repositories.Implementation
{
    public class ContactStudentParent : IContactStudentParent
    {
        private readonly EduProfileDbContext _context;
        private readonly WhatsAppHelper _whatsAppHelper;

        public ContactStudentParent(EduProfileDbContext context, WhatsAppHelper whatsAppHelper)
        {
            _context = context;
            _whatsAppHelper = whatsAppHelper;
        }

        public async Task<ContactStudentParentViewModel> GetParentDetailsByStudentId(Guid studentId)
        {
            var student = await _context.Student
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null)
                return null;

            var parent = await _context.Parent
                .FirstOrDefaultAsync(p => p.ParentId == student.ParentId);

            if (parent == null)
                return null;

            return new ContactStudentParentViewModel
            {
                StudentId = student.StudentId,
                ParentId = parent.ParentId
            };
        }

        public async Task<(bool, string)> SendMessageToParent(ContactStudentParentViewModel model)
        {
            var student = await _context.Student.FirstOrDefaultAsync(s => s.StudentId == model.StudentId);
            if (student == null)
                return (false, "Student not found.");

            var parent = await _context.Parent.FirstOrDefaultAsync(p => p.ParentId == model.ParentId);
            if (parent == null)
                return (false, "Parent not found.");

            var phoneNumber = parent.Parent1CellPhone; // Get phone number from the database

            // Ensure the phone number is formatted with the country code
            if (!phoneNumber.StartsWith("+"))
            {
                // Assuming South Africa country code (+27)
                if (phoneNumber.StartsWith("0"))
                {
                    phoneNumber = "+27" + phoneNumber.Substring(1);
                }
                else
                {
                    phoneNumber = "+27" + phoneNumber;
                }
            }

            if (string.IsNullOrEmpty(phoneNumber))
            {
                return (false, "Parent phone number is null or empty.");
            }

            var parentName = parent.Parent1Name; // Assuming you have a ParentName field in your Parent entity
            var studentName = student.FirstName + " " + student.LastName; // Assuming you have FirstName and LastName fields in your Student entity
            var messageContent = model.Message;

            return await _whatsAppHelper.SendTemplateMessage(phoneNumber, parentName, studentName, messageContent);
        }

    }
}
