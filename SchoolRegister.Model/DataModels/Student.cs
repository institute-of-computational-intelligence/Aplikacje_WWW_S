using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SchoolRegister.BLL.DataModels
{
    public class Student : User
    {
        public IList<Grade> Grades {get; set; }
        public Group Group { get; set; }
        public int? GroupId { get; set; }
        public Parent Parent { get; set; }
        public int? ParentId { get; set; } 

        [NotMapped]
        public double AverageGrade => Grades == null || Grades.Count == 0 ? 0.0d : Math.Round(Grades.Average(g => (int) g.GradeValue), 1);
        [NotMapped]
        public IDictionary<string, double> AverageGradePerSubject => Grades == null ? new Dictionary<string, double>() : Grades
            .GroupBy(g => g.Subject.Name)
            .Select(g => new {SubjectName = g.Key, AvgGrade = Math.Round(g.Average(avg => (int) avg.GradeValue), 1)})
            .ToDictionary(avg => avg.SubjectName, avg => avg.AvgGrade);

        [NotMapped]
        public IDictionary<string, List<GradeScale>> GradesPerSubject => Grades == null ? new Dictionary<string, List<GradeScale>>() : Grades
            .GroupBy(g => g.Subject.Name)
            .Select(g => new { SubjectName = g.Key, GradeList = g.Select(x => x.GradeValue).ToList()})
            .ToDictionary(x => x.SubjectName, x => x.GradeList);
    }
}