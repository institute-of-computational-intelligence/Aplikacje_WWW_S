using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SchoolRegister.Model.DataModels
{
    public class Student : User
    {
        public virtual Group Group { get; set; }
        [ForeignKey("Group")]
        public int? GroupId { get; set; }

        public virtual Parent Parent { get; set; }
        [ForeignKey("Parent")]
        public int? ParentId { get; set; }

        public virtual IList<Grade> Grades { get; set; }

        [NotMapped]
        public double AverageGrade => Grades is null || Grades.Count == 0 ? 0.0d : Math.Round(Grades.Average(x => (int)x.GradeValue), 1);

        [NotMapped]
        public IDictionary<string, double> AverageGradePerSubject
        {
            get
            {
                if (Grades is null)
                    return new Dictionary<string, double>();

                return Grades.GroupBy(g => g.Subject.Name)
                    .Select(g => new { SubjectName = g.Key, AvgGrade = Math.Round(g.Average(x => (int)x.GradeValue), 1) })
                    .ToDictionary(avg => avg.SubjectName, avg => avg.AvgGrade);
            }
        }




        public IDictionary<string, List<GradeScale>> GradesPerSubject
        {
            get
            {
                if (Grades is null)
                    return new Dictionary<string, List<GradeScale>>();

                return Grades.GroupBy(g => g.Subject.Name)
                    .Select(g => new { SubjectName = g.Key, GradeList = g.Select(x => x.GradeValue).ToList() })
                    .ToDictionary(x => x.SubjectName, x => x.GradeList);
            }
        }


    }

}