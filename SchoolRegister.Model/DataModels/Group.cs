using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SchoolRegister.Model.DataModels
{
    public class Group
    {

        [Key]
        public int Id {get; set;}
        [Required]
        public string Name {get; set;}
        public IList<Student> Students {get; set;}
        public IList<SubjectGroup> SubjectGroups {get; set;}
    }
}