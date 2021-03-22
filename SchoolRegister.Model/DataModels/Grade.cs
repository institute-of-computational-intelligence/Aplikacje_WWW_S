using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.DataModels
{
    public class Grade
    {
             [Key]
        public DateTime DateOfIssue {get; set;}
        public GradeScale GradeValue {get; set;}
        public Student Student {get; set;}
        [ForeignKey("Stuent")]
        public int StudentId {get; set;}
        public Subject Subject {get; set;}
        [ForeignKey("Subject")]

        public int SubjectId {get; set;}
    }
}