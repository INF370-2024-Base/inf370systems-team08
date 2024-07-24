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
            var parent = await _context.Parent.FirstOrDefaultAsync(p => p.ParentId == model.ParentId);
            if (parent == null)
                return (false, "Parent not found.");

            var message = model.Message;
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

            return await _whatsAppHelper.SendMessage(phoneNumber, message);
        }
    }
}
