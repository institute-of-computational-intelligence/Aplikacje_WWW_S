using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.DataModels
{
    public class Grade
    {
        [Key]
        public DateTime DateOfIssue { get; set; }
        public int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public int StudentId { get; set; }
        [ForeignKey("StudentId")]

        public virtual GradeScale GradeValue { get; set; }
        public virtual Student Student { get; set; }
        public virtual Subject Subject { get; set; }
        
    }
}
