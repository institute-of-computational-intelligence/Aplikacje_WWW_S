using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.BLL.DataModels
{
    public class Grade
    {
        public DateTime DateOfIssue { get; set; }
        public GradeScale GradeValue { get; set; }
        public Student Student { get; set; }
        public int StudentId { get; set; }
        public Subject Subject { get; set; }
        public int SubjectId { get; set; }
    }
}