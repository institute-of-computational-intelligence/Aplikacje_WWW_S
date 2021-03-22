using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.DataModels
{
    public class Teacher: User
    {
        public virtual IList<Subject> Subjects { get; set; }
        [Required]
        public string Title { get; set; }
    }
}