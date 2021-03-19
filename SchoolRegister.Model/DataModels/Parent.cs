using System;
using System.Collections.Generic;

namespace SchoolRegister.BLL.DataModels
{
    public class Parent : User
    {
        public virtual IList<Student> Students { get; set; }
        
    }
}