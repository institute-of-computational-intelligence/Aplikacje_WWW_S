using Microsoft.AspNetCore.Identity;
using System;

namespace SchoolRegister.Model.DataModels
{
    public class Role : IdentityRole<int>
    {
        public RoleValue roleValue {get; set;}

        public Role() {}
        public Role(string name, RoleValue roleValue):base(name)
        { 
            this.roleValue = roleValue;
        }
    }    
}