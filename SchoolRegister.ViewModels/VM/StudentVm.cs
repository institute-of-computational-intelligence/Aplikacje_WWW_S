using System.ComponentModel.DataAnnotations;
using System;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.ViewModels.VM
{
    public class StudentVm
    {
        [Required]
        public int StudentId{get; set;}
        [Required]
        public int GroupId{get; set;}

        
        

    }
}