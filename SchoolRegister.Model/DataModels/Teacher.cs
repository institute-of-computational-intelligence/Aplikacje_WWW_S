using System.Collections.Generic;

namespace SchoolRegister.Model.DataModels
{
    public class Teacher : User
    {
        public virtual IList<Subject> Subcjects {get; set;}
        public string Title {get; set;}
        
    }
}