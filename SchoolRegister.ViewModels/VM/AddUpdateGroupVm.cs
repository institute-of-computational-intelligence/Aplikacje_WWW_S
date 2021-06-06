using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SchoolRegister.ViewModels.VM
{
    public class AddUpdateGroupVm
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}