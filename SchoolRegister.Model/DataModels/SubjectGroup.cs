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

        public Group Group { get; set; }
        public Subject Subject { get; set; }
    }
}