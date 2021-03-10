using Microsoft.AspNetCore.Identity;
using System;

namespace SchoolRegister.Model.DataModels
{
    public class SubjectGroup
    {
        public Group group {get; set;}
        public int groupId {get; set;}
        public Subject subject {get; set;}
        public int subjectId {get;set;}
    }
}