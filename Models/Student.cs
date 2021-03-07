using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zadanie_1.Models
{
    public class Student : User
    {
        public double AverageGrade { get; }
        public IDictionary<string, double> AverageGradePerSubject { get; }
        public IList<Grade> Grades { get; set; }
        public IDictionary<string, List<GradeScale>> GradesPerSubject {get;}
        public Group Group { get; set; }
        public Parent Parent { get; set; }
        public int? ParentId { get; set; }

    }
}
