using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
    public class AttachDetachSubjectToTeacherVm
    {
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public int TeacherId { get; set; }
    }
}
