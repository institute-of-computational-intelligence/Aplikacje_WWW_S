using Microsoft.AspNetCore.Identity;

namespace SchoolRegister.Model.DataModels
{
    public class Role : IdentityRole<int>
    {
        public RoleValue RoleValue { get; set; }

        Role()
        {

        }
        Role(string name, RoleValue roleValue)
        {
            RoleValue = roleValue;
        }
    }
}