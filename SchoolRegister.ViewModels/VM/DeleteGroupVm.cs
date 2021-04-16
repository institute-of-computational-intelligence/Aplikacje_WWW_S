using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace SchoolRegister.ViewModels.VM
{
    public class DeleteGroupVm
    {
        [Required]
        public int Id { get; set; }
    }
}