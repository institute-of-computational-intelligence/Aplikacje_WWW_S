using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SchoolRegister.Model.DataModels
{
    public enum RoleValue
    {
        User = 0,
        Student = 1,
        Parent = 2,
        Teacher = 3,
        Admin = 4,
    }
}