
using System;

namespace SchoolRegister.BLL.DataModels
{
    public class Group
    {
        public int Id {get;set;}

        public string Name{get;set;}

        public IList<Student> Students{get;set;}

        public IList<SubjectGroups> SubjectGroups {get;set;}
    }
}