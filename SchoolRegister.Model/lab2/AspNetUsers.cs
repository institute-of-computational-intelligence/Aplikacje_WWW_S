using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.lab2
{
    public class AspNetUsers
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string UserType { get; set; }
        [ForeignKey("Group")]
        public int GroupId { get; set; }
        public int ParentId { get; set; }
        public string title { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public string EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string LockoutEnd { get; set; }
        public string LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
    }
}