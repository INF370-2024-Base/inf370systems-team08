using EduProfileAPI.Repositories.Interfaces;
using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using EduProfileAPI.WhatsApp;
using EduProfileAPI.Models;
using Newtonsoft.Json;

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

        public async Task<(bool, string)> SendMessageToParent(ContactStudentParentViewModel model, Guid userId)
        {
            var student = await _context.Student.FirstOrDefaultAsync(s => s.StudentId == model.StudentId);
            if (student == null)
                return (false, "Student not found.");

            var parent = await _context.Parent.FirstOrDefaultAsync(p => p.ParentId == model.ParentId);
            if (parent == null)
                return (false, "Parent not found.");

            var phoneNumber = parent.Parent1CellPhone;
            if (!phoneNumber.StartsWith("+"))
            {
                phoneNumber = phoneNumber.StartsWith("0") ? "+27" + phoneNumber.Substring(1) : "+27" + phoneNumber;
            }

            if (string.IsNullOrEmpty(phoneNumber))
            {
                return (false, "Parent phone number is null or empty.");
            }

            var parentName = parent.Parent1Name;
            var studentName = student.FirstName + " " + student.LastName;
            var messageContent = model.Message;

            var (success, messageResponse) = await _whatsAppHelper.SendTemplateMessage(phoneNumber, parentName, studentName, messageContent);

            // Log action in audit trail
            var auditTrail = new AuditTrail
            {
                UserId = userId,
                Action = "SEND_MESSAGE",
                EntityName = "Parent",
                AffectedEntityID = parent.ParentId,
                OldValue = null, // No old value since it's a send action
                NewValue = JsonConvert.SerializeObject(new { student.StudentId, parent.ParentId, Message = messageContent }),
                TimeStamp = DateTime.UtcNow
            };
            await _context.AuditTrail.AddAsync(auditTrail);
            await _context.SaveChangesAsync();

            return (success, messageResponse);
        }

    }
}
