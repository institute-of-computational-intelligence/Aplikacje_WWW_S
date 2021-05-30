using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace SchoolRegister.ViewModels.VM
{

    public class EmailRequestVm
    {
        [Required]
        public int TeacherId { get; set; }
        [Required]
        public int ParentId { get; set; }
        [Required]
        public string EmailSubject { get; set; }
        [Required]
             
        public string EmailBody { get; set; }

        public IFormFileCollection Attachments { get; set; }
    }
        
}        