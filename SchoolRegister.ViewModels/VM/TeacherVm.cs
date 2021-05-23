using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SchoolRegister.ViewModels.VM
{
    public class TeacherVm
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Id { get; set; }
        public int GroupId { get; set; }
        public IEnumerable<Group> Groups { get; set; }
    }
}