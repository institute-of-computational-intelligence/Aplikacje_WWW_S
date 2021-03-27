using System;

namespace SchoolRegister.Model.DataModels
{
    public class Grade
    {
        public DateTime DateOfIssue { get; set; }
        public GradeScale GradeValue {get; set; }
        public virtual Student Student { get; set; }
        public int StudentId { get; set; }
        public virtual Subject Subject { get; set; }
        public int SubjectId {get; set; }
    }
}