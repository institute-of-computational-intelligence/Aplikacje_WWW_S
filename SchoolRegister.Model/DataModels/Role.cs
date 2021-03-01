using Microsoft.AspNetCore.Identity;

namespace SchoolRegister.BLL.DataModels
{
    public class Role: IdentityRole<int>
    {
        public RoleValue RoleValue { get; set; }
        public Role() {}
        public Role(string name, RoleValue rv): base(name)
        {
            this.RoleValue = rv;
        }
    }
}