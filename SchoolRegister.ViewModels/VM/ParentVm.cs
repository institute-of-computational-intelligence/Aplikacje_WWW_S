using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.ViewModels.VM {
    public class ParentVm {
        public int Id { get; set; }
        
        [Display(Name="Name")]
        public string ParentName { get; set; }
        public IEnumerable<Student> Students { get; set; }
    }

       
}         