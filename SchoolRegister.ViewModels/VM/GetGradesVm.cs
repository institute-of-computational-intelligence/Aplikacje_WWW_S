using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
    public class GetGradesVm
    {
        [Required]
        public int CallerId { get; set; }

        [Required]
        public int StudentId { get; set; }
    }
}