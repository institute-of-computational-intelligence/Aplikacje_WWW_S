using System.ComponentModel.DataAnnotations;
using System;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.ViewModels.VM
{
    public class AddGradeToStudentVm
    {
        public int? StudentId { get; set; }
        public int? TeacherId { get; set; }

        [Required]
        public DateTime DateOfIssue { get; set; }

        [Required]
        public GradeScale GradeValue { get; set; }

    }
}