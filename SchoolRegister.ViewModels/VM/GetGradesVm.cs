using System;
using System.ComponentModel.DataAnnotations;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.ViewModels.VM
{
    public class GetGradesVm
    {
        [Required]
        public int UserId {get; set;}
        [Required]
        public int StudentId {get; set;}
    }
}