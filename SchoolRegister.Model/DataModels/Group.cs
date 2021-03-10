using Microsoft.AspNetCore.Identity;
using System;

namespace SchoolRegister.Model.DataModels
{
    public class Group
    {
        public int id {get; set;}
        public string name {get; set;}
        public IList<Student> students {get; set;}
        public IList<SubjectGroup> subjectGroups {get; set;} 
    }    
}