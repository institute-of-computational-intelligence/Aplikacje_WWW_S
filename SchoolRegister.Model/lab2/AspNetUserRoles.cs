using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.lab2
{
    public class AspNetUserRoles
    {
        [ForeignKey("AspNetUsers")]
        public int UserId { get; set; }
        [ForeignKey("AspNetRoles")]
        public int RoleId { get; set; }
    }
}