using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolRegister.BLL.DataModels
{
    public class Student:User
    {
        public Group Group {get; set;}
        public int? GroupId {get; set;}
        public IList<Grade> Grades {get; set;}
        public Parent Parent {get; set;}
        public int? ParentId {get; set;}
        public double AverageGrade => Grades == null || Grades.Count == 0 ? 0.0d : Math.Round(Grades.Average(global => (int)global.GradeValue), 1);
        public IDictionary<string, double> AverageGradePerSubject => Grades == null ? new Dictionary<string, double>() : Grades
            .GroupBy(g => g.Subject.Name)
            .Select(g => new {SubjectName = g.Key, AvgGrade = Math.Round(g.Average(avg => (int) avg.GradeValue), 1)})
            .ToDictionary(avg => avg.SubjectName, avg => avg.AvgGrade);
        public IDictionary<string, List<GradeScale>> GradesPerSubject => Grades == null ? new Dictionary<string, List<GradeScale>>() : Grades
            .GroupBy(g => g.Subject.Name)
            .Select(g => new {SubjectName = g.Key, GradeList = g.Select(x=>x.GradeValue).ToList()})
            .ToDictionary(x=>x.SubjectName, x=>x.GradeList); 


    }
}