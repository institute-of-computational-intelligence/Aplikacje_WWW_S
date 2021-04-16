using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
    public class SendMailVm
    {
        [Required]
        public int FromId { get; set; }

        [Required]
        public int ToId { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
    }
}