using System;
using System.Collections.Generic;


namespace SchoolRegister.Model.DataModels {
    class Teacher : User {
        public List<Subject> Subjects {get; set;}
        public string Title {get;set;}
    }
}