using System;
using System.ComponentModel.DataAnnotations;

namespace EduProfileAPI.ViewModels
{
    public class CreateDisciplinaryTypeVM
    {
        public Guid DisciplinaryTypeId { get; set; }

        [StringLength(50)]
        public string DisciplinaryTypeName { get; set; }

        [StringLength(200)]
        public string DisciplinaryTypeDescription { get; set; }
    }
}
