using System.ComponentModel.DataAnnotations;
using SchoolRegister.BLL.DataModels;

namespace SchoolRegister.ViewModels.VM
{
    public class AddGradeToStudentVm
    {
        [Required]
        public GradeScale Grade { get; set; }
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public int TeacherId { get; set; }
    }
}