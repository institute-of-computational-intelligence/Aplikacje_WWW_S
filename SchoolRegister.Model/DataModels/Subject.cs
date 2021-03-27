using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.DataModels
{
    public class Subject
    {
        public string Description {get;set;}
        public virtual IList<Grade> Grades {get;set;}
        public int Id{get;set;}
        [Required]
        public string Name{get;set;}
        public virtual IList<SubjectGroup> SubjectGroups {get;set;}
        public virtual Teacher Teacher {get;set;}
        public int? TeacherId{get;set;}
    }
}