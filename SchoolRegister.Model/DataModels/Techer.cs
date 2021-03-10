using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SchoolRegister.Model.DataModels
{
    public class Teacher: User
    {
        public IList<Subject> Subjects { get; set; }
        public string Title { get; set; }
    }
}