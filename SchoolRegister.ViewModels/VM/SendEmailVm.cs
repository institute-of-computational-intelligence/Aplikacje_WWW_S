using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
    public class SendEmailVm
    {
        [Required]
        public int SenderId { get; set; }
        
        [Required]
        public int RecipientId { get; set; }
        
        public string EmailSubject { get; set; }

        public string EmailBody { get; set; }
    }
}