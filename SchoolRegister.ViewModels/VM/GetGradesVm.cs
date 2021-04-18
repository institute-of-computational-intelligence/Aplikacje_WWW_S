using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
    public class GetGradesVm
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int StudentId { get; set; }
    }
}