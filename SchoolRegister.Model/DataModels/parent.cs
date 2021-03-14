using System;
using System.Collections.Generic;

namespace SchoolRegister.Model.DataModels {
    class Parent : User {
        public List<Student> Students {get; set;}
    }
}