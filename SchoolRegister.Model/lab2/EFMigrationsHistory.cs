using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.lab2
{
    public class EFmigrationsHistory
    {
        [Key]
        public int MigrationId { get; set; }
        public string ProductVersion { get; set; }
    }
}