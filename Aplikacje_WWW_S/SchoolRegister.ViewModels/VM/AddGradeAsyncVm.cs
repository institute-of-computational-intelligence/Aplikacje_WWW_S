using System.ComponentModel.DataAnnotations;
using System;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.ViewModels.VM
{
    public class AddGradeAsyncVm
    {

        public int? TeacherId { get; set; }
        [Required]
        public GradeScale GradeValue { get; set; }
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int SubjectId { get; set; }

    }
}