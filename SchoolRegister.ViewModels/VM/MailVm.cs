using System;
using System.ComponentModel.DataAnnotations;


namespace SchoolRegister.ViewModels.VM
{
    public class MailVm
    {
        public int? TeacherId {get;set;}
        public int? UserId {get;set;}
        [Required]
        public string Title {get;set;}
        [Required]
        public string Content {get;set;}
    }
}