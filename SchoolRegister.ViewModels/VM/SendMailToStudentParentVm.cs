using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
    public class SendMailToStudentParentVm
    {   
        
        public int? TeacherId { get; set; }
        public int? ParentId { get; set; }
        [Required]
        public string MailTitle { get; set; }
        [Required]
        public string MailContent {get; set; }
    }
}