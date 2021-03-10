using System;

namespace SchoolRegister.Model.DataModels
{
    public class Role
    {
        public RoleValue RoleValue { get; set; }
        public Role()
        {

        }
        public Role(string name, RoleValue roleValue)
        {
            this.RoleValue = roleValue;
        }
    }
}