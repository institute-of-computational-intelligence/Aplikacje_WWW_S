using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SchoolRegister.Model.DataModels
{
    public class Role : IdentityRole<int>
    {
        public RoleValue RoleValue { get; set; }

        private Role()
        {
            
        }

        private Role(string name, RoleValue roleValue)
        {
            
        }
    }
}