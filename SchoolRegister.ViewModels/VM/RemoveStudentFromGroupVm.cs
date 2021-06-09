using System.ComponentModel.DataAnnotations;
using SchoolRegister.BLL.DataModels;

namespace SchoolRegister.ViewModels.VM
{
    public class RemoveStudentFromGroupVm
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int GroupId { get; set; }
    }
}