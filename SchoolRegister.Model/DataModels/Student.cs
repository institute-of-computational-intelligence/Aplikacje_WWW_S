using System.Collections.Generic;
using System;

namespace SchoolRegister.BLL.DataModels
{
    public class Student : User
    {
        public double AverageGrade { get; }
        public IDictionary<string, double> AverageGradePerSubject { get; }
        public virtual IList<Grade> Grades { get; set; }
        public IDictionary<string, List<GradeScale>> GradesPerSubject { get; }
        public virtual Group Group { get; set; }
        public int? GroupId { get; set; }
        public virtual Parent Parent { get; set; }
        public int? ParentId { get; set; }
    }
}