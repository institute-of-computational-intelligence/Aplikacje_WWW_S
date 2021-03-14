using System;
using Microsoft.AspNetCore.Identity;

namespace SchoolRegister.Model.DataModels {
    enum RoleValue {User, Student, Parent, Teacher, Admin}
    class Role : IdentityRole<int> {
        public RoleValue RoleValue {get;set;}

        public Role() {}

        public Role(string name, RoleValue roleValue) {}
        
    }
}