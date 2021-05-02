using System;
using SchoolRegister.Model.DataModels;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{  public class AddGradeToStudentVm
    {
        [Required]
        public int? TeacherId {get;set;}

        [Required]
        public GradeScale GradeValue {get;set;}

        [Required]
        public int StudentId {get;set;}

        [Required]
        public int SubjectId {get;set;}
    }
}