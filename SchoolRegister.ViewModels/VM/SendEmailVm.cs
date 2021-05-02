using System.ComponentModel.DataAnnotations;
using System;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.ViewModels.VM
{
    public class SendEmailVm
    {
        [Required]
        public int RecipientId{get; set;}
        [Required]
        public int SenderId{get; set;}
        [Required]
        public string MailContent{get; set;}
        [Required]
        public string EmailTitle{get; set;}
        
        

    }
}