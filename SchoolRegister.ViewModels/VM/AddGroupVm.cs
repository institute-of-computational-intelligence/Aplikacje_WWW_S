using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolRegister.ViewModels.VM
{
    public class AddGroupVm
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
    }
}
