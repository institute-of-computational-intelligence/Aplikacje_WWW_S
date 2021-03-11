using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.lab2
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Descritption { get; set; }
        public string TeacherId { get; set; }
    }
}