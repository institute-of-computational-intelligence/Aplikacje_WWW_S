using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SchoolRegister.BLL.DataModels
{
    public class Parent : User
    {
        public IList<Student> Students { get; set; }
    }
}