using Microsoft.AspNetCore.Identity;
using System;

namespace SchoolRegister.BLL.DataModels
{
    public class Grade
    {
        public DateTime DateOfissue {get; set;}
        public GradeScale GradeValue {get; set;}
        public Student Student {get; set;}
        public int StudentId {get; set;}
        public Subject Subject {get; set;}
        public int SubjectId {get; set;}
    }
}