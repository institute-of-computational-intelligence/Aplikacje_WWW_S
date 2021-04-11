using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
    public class ShowGradesVm
    {
        [Required]
        public int StudentOrParentId { get; set; }

        [Required]
        public int StudentId { get; set; }
    }
}