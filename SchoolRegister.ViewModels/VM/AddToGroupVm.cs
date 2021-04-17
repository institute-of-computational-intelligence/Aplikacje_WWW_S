using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
    public class AddToGroupVm
    {
        [Required]
        public int GroupId { get; set; }
        [Required]
        public int StudentId { get; set; }
    }
}