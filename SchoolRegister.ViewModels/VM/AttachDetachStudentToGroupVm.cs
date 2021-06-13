using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
   public class AttachDetachStudentToGroupVm
    {
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int GroupId { get; set; }
    }
}