using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.ViewModels.VM
{
    public class AddGradeVm
    {
        [Required]
        public int teacherId { get; set; }
        [Required]
        public DateTime DateOfIssue { get; set; }
        [Required]
        public GradeScale GradeValue { get; set; }
        [Required]    
        public int StudentId { get; set; }
        [Required]
        public int SubjectId { get; set; }
        

    }
}