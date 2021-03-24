using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.DataModels
{
    public class SubjectGroup
    {
        [Key]
        public int GroupId { get; set; }
        [Key]
        public int SubjectId { get; set; }

        public virtual Group Group { get; set; }
        public virtual Subject Subject { get; set; }
    }
}