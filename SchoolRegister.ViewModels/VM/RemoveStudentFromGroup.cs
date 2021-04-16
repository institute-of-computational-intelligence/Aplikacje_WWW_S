using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
    public class RemoveStudentVm
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int GroupId { get; set; }
    }
}