using Microsoft.AspNetCore.Identity;
using System;

namespace SchoolRegister.Model.DataModels
{
    public class Parent : User
    {
       public IList<Student> students  {get; set;}
    }
}