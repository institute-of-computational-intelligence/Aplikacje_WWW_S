using System.Collections.Generic;

namespace SchoolRegister.Model.DataModels
{
    public class Teacher{
        public IList<Subject> Subjects{get;set;}
        public string Title{get;set;}
    }
}