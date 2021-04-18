using System.ComponentModel.DataAnnotations;
using System;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.ViewModels.VM
{
    public class SendEMailVm
    {
        [Required]
        public int ParentId{get; set;}
        [Required]
        public int TeacherId{get; set;}
        [Required]
        public string Content{get; set;}
        [Required]
        public string Title{get; set;}
        
        

    }
}