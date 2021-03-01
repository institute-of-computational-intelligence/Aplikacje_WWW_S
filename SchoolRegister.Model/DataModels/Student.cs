using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;  

using System;

namespace SchoolRegister.Model.DataModels
{
    public class Student : User
    {
        public double AverageGrade {get;}
        public IDictionary<string, double> AverageGradePerSubject {get;}
        public IList<Grade> Grades {get; set;}
        public IDictionary<string, List<GradeScale>> GradesPerSubject {get;}
        public Group Group {get; set;}
        public int? GrupID {get; set;}
        public Parent Parent {get; set;}
        public int? ParentId {get; set;}
    }
}