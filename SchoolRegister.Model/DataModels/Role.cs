using Microsoft.AspNetCore.Identity;

namespace SchoolRegister.Model.DataModels{
    
    public class Role : IdentityRole<int>
    {
        RoleValue RoleValue{get;set;}

        public Role(){}

        public Role(string name,RoleValue roleValue):base(name)
        {
            this.RoleValue=roleValue;
        }
    }
}