using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
    public class AddGroupVm
    {
        [Required]
        public string Name { get; set; }
    }
} 