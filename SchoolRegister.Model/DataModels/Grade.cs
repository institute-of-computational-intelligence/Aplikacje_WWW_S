using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.DataModels
{
    public class Grade
    {   
        [Key]
        public DateTime DateOfIssue{get;set;}

        public virtual GradeScale GradeValue{get;set;}

        public virtual Student Student{get;set;}
        [ForeignKey("Student")]
        public int StudentId{get;set;}

        public virtual Subject Subject {get;set;}
        [ForeignKey("Subject")]
        public int SubjectId{get;set;}  
    }
}