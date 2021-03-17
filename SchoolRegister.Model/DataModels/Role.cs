using Microsoft.AspNetCore.Identity;
using System;

namespace SchoolRegister.Model.DataModels
{
    public class Role : IdentityRole<int>
    {
       public RoleValue RoleValue { get; set; }

       public Role() {}
       public Role(string name, RoleValue roleValue)
       {
           base.Id = (int)roleValue;
           base.Name = name;

           RoleValue = roleValue;     
       }
 
    }

}