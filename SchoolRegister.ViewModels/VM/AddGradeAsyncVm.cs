using System.ComponentModel.DataAnnotations;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.ViewModels.VM
{
    public class AddGradeAsyncVm
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int TeacherId { get; set; }

        [Required]
        public int SubjectId { get; set; }

        [Required]
        public GradeScale GradeValue { get; set; }
    }
}
