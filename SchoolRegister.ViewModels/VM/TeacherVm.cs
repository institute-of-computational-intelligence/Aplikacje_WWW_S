using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolRegister.ViewModels.VM
{
   public class TeacherVm
    {
        public int Id { get; set; }
        public IList<SubjectVm> Subjects { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
