using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SchoolReggister.Model.DataModels
{
    public class Grade
    {
        public DateTime DateofIssue {get; set;}
        public GradeScale GradeValue {get; set;}
        public Student Student {get; set;}
        public int StudentId {get; set;}
        public Subject Subject {get; set;}
        public int SubjectId {get; set;}
    }
}