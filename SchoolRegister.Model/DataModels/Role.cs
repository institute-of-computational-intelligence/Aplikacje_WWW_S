using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SchoolRegister.Model.DataModels
{
    public class Role
    {
        public RoleValue RoleValue{ get; set; }
        public Role()
        {

        }
        public Role(string name, RoleValue roleValue)
        {
            
        }
    }
}