using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.DataModels
{
    public class Teacher : User
    {
        public virtual IList<Subject> Subcjects {get; set;}
        [Required]
        public string Title {get; set;}
        
    }
}