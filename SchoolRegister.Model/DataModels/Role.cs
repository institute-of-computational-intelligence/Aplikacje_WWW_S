using Microsoft.AspNetCore.Identity;

namespace SchoolRegister.Model.DataModels
{
     public class Role : IdentityRole<int>
     {
         public Role()
         {

         }       

         public Role(string name, RoleValue roleValue)
         {
            
         }

         public RoleValue RoleValue { get; set; }
    
     }
}