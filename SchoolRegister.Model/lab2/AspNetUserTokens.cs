using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.lab2
{
    public class AspNetUserTokens
    {
        [ForeignKey("AspNetUsers")]
        public int UserId { get; set; }
        [Key]
        public int LoginProvider { get; set; }
        [Key]
        public int Name { get; set; }
        public int Value { get; set; }
    }
}