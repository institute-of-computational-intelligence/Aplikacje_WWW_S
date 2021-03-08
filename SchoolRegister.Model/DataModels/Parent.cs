using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SchoolReggister.Model.DataModels
{
    public class Parent : User
    {
        public IList<Student> Students {get; set;}
    }
}