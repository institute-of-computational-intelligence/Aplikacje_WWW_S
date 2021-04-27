using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
    public class SendEmailVm
    {
        [Required]
        public int TeacherId { get; set; }

        [Required]
        public int ParentId { get; set; }

        [Required]
        public string EmailSubject { get; set; }

        public string EmailContent { get; set; }
    }
}