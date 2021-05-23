using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
    public class StudentVm
    {
        public int StudentId { get; set; }

        [Display(Name = "Name")]
        public string FirstName { get; set; }

        [Display(Name = "Surname")]
        public string LastName { get; set; }
        public int? GroupId { get; set; }

        public int? ParentId { get; set; }

    }
}