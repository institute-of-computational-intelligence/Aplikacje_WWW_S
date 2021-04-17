using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{

    public class CheckGradesVm
    {
        [Required]
        public int CurrentUserId { get; set; }
        [Required]
        public int StudentId { get; set; }
    }

}