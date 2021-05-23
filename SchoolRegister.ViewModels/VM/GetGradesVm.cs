using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
    public class GetGradesVm
    {
        [Required] public int StudentId { get; set; }

        [Required] public int UserId { get; set; }
    }
}