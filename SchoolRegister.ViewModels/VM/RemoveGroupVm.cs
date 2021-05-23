using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
    public class RemoveGroupVm
    {
        [Required]
        public int Id { get; set; }
    }
}