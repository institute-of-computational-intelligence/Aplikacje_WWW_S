using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.DataModels
{
    public class Grade
    {
        [Key]
        public DateTime DateOfIssue { get; set; }
        public GradeScale GradeValue { get; set; }
        public virtual Student Student { get; set; }
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public virtual Subject Subject { get; set; }
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }

    }
}