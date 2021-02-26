using Microsoft.AspNetCore.Identity;
using System;

namespace SchoolRegister.BLL.DataModels
{
    public class User : IdentityUser<int>
    {
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public DateTime RegistrationDate {get;set;}
    }
}