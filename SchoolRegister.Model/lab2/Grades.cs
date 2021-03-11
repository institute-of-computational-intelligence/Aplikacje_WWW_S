using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.lab2
{
    public class Grades
    {
        public DateTime DateOfIssue { get; set; }
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        [ForeignKey("AspNetUsers")]
        public int StudentId { get; set; }
        public int GradeValue { get; set; }
    }
}