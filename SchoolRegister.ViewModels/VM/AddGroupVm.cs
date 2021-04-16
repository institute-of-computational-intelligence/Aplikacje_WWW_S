using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;


namespace SchoolRegister.ViewModels.VM
{
    public class AddGroupVm
    {
        [Required]
        public string Name { get; set; }
    }
}