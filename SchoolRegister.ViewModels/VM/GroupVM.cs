using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM {
    public class GroupVm {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IList<StudentVm> Students { get; set; }

        public IList<SubjectVm> Subjects { get; set; }
    }
}