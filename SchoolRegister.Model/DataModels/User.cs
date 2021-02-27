using Microsoft.AspNetCore.Identity;
using System;


enum User {daun, daun1, daun2}

namespace SchoolRegister.Model.DataModels
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegistrationDate { get; set; }
        
    }
} 