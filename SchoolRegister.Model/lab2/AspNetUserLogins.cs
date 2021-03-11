using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.lab2
{
    public class AspNetUserLogins
    {
        [Key]
        public int LoginProvider { get; set; }
        public int ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
    }
}