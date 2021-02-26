
using System;

namespace SchoolRegister.BLL.DataModels
{
    public class Parent : User
    {
        public IList<Student> Students {get;set;}
        
    }
}