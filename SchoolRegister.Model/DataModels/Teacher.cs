using System;

namespace SchoolRegister.BLL.DataModels
{
    public class Teacher : User
    {
        public IList<Subject> Subjects {get;set;}
        public string Title {get;set;}
    }
}