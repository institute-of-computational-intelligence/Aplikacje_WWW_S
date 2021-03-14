using System;
using System.Collections.Generic;

namespace SchoolRegister.Model.DataModels {
    class Student : User{
        public double AverageGrade {get; }
        public Dictionary<string, double> AverageGradePerSubject {get;}
        public List<Grade> Grades {get; set;}
        public Dictionary<string, List<GradeScale>> GradePerSubject {get;}
        public Group Group {get; set;}
        public int? GroupId {get; set;}
        public Parent Parent {get;set;}
        public int? ParentId {get;set;}
    }
}