using Microsoft.AspNetCore.Identity;
using System;

namespace SchoolRegister.Model.DataModels
{
    public enum RoleValue : int
    {
        User,
        Student,
        Parent,
        Teacher,
        Admin
    }
}