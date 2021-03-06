using Microsoft.AspNetCore.Identity;
using System;

namespace SchoolRegister.BLL.DataModels
{
    public class SubjectGroup
    {
        public SubjectGroup Group {get; set;}
        public int GroupId {get; set;}
        public Subject Subject {get; set;}
        public int SubjectId {get; set;}
    }
}