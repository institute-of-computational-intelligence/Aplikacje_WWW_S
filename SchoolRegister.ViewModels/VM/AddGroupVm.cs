using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
    public class AddGroupVm
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
    }
}