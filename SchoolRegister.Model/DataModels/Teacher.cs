using System.Collections.Generic;
using System;

namespace SchoolRegister.BLL.DataModels
{
    public class Teacher : User
    {
        public virtual IList<Subject> Subject { get; set; }
        public string Title { get; set; }
    }
}