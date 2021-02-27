using System;

namespace SchoolRegister.Model.DataModels
{
    public class Parent : User
    {
        public IList<Student> Students { get; set; }
    }
}