using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SchoolRegister.Model.DataModels
{
    public class Parent : User
    {
        public virtual IList<Student> Students { get; set; }
    }
}