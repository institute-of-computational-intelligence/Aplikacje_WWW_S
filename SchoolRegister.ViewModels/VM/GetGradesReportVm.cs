using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
    public class GetGradesReportVm
    {
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int GetterUserId { get; set; }
    }
}
