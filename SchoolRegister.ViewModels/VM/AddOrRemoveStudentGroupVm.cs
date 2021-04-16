using System;
using SchoolRegister.Model.DataModels;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
    public class AddOrRemoveStudentGroupVm
    {
        [Required]
        public int StudentId {get; set;}
        [Required]
        public int GroupId {get; set;}
    }
}