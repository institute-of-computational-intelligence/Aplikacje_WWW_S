using System;
using System.Collections.Generic;

namespace SchoolRegister.Model.DataModels
{
    public class Student : User
    {
        public virtual IList<Grade> Grades { get; set; }
        public virtual Group Group { get; set; }
        public virtual Parent Parent { get; set; }

        public double AverageGrade { get; }
        public IDictionary<string, double> AverageGradePerSubject { get; }
        public IDictionary<string, List<GradeScale>> GradesPerSubject { get; }
        public int? GroupId { get; set; }
    }
}