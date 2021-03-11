using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.lab2
{
    public class AspNetUserClaims
    {
        public int Id { get; set; }
        [ForeignKey("AspNetUsers")]
        public int UserId { get; set; }
        public string ClaimType { get; set; }
        public int ClaimValue { get; set; }
    }
}