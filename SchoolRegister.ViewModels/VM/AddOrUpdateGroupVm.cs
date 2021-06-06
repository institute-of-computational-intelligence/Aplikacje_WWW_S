using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SchoolRegister.ViewModels.VM
{
   public class AddOrUpdateGroupVm
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; } 

    }
}
