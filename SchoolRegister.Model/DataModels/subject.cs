using System;
using System.Collections.Generic;


namespace SchoolRegister.Model.DataModels {
    class Subject{
        public string Description {get; set; }
        public List<Grade> Grades {get; set;}
        public int Id {get; set;}
        public string Name {get; set; }
        public List<SubjectGroup> SubjectGroups {get; set;}
        public Teacher Teacher {get;set;}
        public int? TeacherId {get;set;}
    }
}