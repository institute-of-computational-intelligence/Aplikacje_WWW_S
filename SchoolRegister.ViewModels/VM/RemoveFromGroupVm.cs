using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace SchoolRegister.ViewModels.VM
{
    public class RemoveFromGroupVm
    {
        [Required]
        public int StudentId { get; set; }
    }
}