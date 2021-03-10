using Microsoft.AspNetCore.Identity;
using System;

namespace SchoolRegister.Model.DataModels
{    
    public class Grade
    {
        public DateTime dateOfIssue {get; set;}
        public GradeScale gradeValue {get; set;}
        public Student student {get; set;}
        public int studentId {get; set;}
        public Subject subject {get; set;}
        public int subjectId {get; set;}
    }

}