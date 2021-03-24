using System;
/*
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
*/
namespace SchoolRegister.Model.DataModels
{
    public class Grade
    {
        //[Key]
        public DateTime DateOfIssue { get; set; }
        //[Key]
        public int StudentId { get; set; }
        //[Key]
        public int SubjectId { get; set; }

        public virtual GradeScale GradeValue { get; set; }
        public virtual Student Student { get; set; }
        public virtual Subject Subject { get; set; }
    }
}