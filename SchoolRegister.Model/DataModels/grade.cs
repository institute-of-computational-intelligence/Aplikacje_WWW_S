using System;
using System.Collections.Generic;
namespace SchoolRegister.Model.DataModels {
    enum GradeScale {NDST, DST, DB, BDB}
    class Grade {
        public DateTime DateOfIssue {get; set; }
        public GradeScale GradeValue {get; set;}
        public Student Student {get; set;}
        public int StudentId {get; set;}
        public Subject Subject {get;set;}
        public int SubjectId {get;set;}
    }
}