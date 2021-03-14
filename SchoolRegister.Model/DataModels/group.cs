using System;
using System.Collections.Generic;


namespace SchoolRegister.Model.DataModels {
     class Group {
        public int Id {get; set; }
        public string Name {get; set;}
        public List<Student> Students {get; set;}
        public List<SubjectGroup> SubjectGroups {get;set;}
        
    }
}