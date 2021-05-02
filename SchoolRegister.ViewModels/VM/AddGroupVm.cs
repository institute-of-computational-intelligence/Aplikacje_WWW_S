using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
    public class AddGroupVm
    {
        public int Id {get; set;}
        [Required]
        public string Name {get; set;}
    }
}