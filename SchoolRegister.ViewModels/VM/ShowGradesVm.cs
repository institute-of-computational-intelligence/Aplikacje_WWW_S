using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
    public class ShowGradesVm
    {
        [Required]
        public int PersonId { get; set; }

        [Required]
        public int StudentId { get; set; }
    }
}