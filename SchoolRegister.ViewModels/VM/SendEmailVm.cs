using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
    public class SendEmailVm
    {
        [Required]
        public int SenderId { get; set; }

        [Required]
        public int RecipientId { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
    }
}