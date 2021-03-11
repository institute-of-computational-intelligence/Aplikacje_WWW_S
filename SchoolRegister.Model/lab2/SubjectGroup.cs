using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.lab2
{
    public class SubjectGroup
    {
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        [ForeignKey("Group")]
        public string GroupId { get; set; }

    }
}