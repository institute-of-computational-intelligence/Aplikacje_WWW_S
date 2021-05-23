using System.Collections.Generic;

namespace SchoolRegister.ViewModels.VM
{
    public class TeacherVm
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public int Id { get; set; }
        public IList<SubjectVm> Subjects { get; set; }
    }
}
