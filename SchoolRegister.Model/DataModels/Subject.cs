using Microsoft.AspNetCore.Identity;
using System;

namespace SchoolRegister.Model.DataModels
{
    public class Subject
    {
        public string description {get; set;}
        public IList<Grade> grades {get; set;}
        public int id {get; set;}
        public string name {get; set;}
        public IList<SubjectGroup> subjectGroups {get; set;}
        public Teacher teacher {get; set;}
        public int? teacherId {get; set;}
    }
}