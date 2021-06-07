using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
    public class RemoveFromGroupVm
    {
        [Required]
        public int StudentId { get; set; }
    }
}
