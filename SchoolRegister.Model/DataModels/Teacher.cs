using System.Collections.Generic;
using System;

namespace SchoolRegister.BLL.DataModels
{
    public class Teacher : User
    {
        public IList<Subject> Subject { get; set; }
        public string Title { get; set; }
    }
}