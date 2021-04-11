using System;
using SchoolRegister.Model.DataModels;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
    public class AddGradeVm
    {
        [Required]
        public int teacherId {get; set;}
        [Required]

        public DateTime DateOfissue {get; set;}
        [Required]
        public GradeScale GradeValue {get; set;}
        [Required]
        public int StudentId {get; set;}
        [Required]
        public int SubjectId {get; set;}
    }
}