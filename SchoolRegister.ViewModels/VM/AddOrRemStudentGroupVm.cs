using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
    public class AddOrRemStudentGroupVm
    {
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int GroupId { get; set; }

    }
}