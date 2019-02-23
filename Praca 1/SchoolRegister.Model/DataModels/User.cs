using Microsoft.AspNetCore.Identity:
using System;

namespace SchoolRegister.Model.DataModels
{

    public class User : IdentityUser<int>
    {
        
        public string FirstName { get; get; }

        public string LastName { get; get; }

        public Datetime RegistrationDate { get; get; }
    }
}
