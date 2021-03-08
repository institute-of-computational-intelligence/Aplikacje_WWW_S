using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zadanie_1.Models
{
    public class Subject
    {
        public string Description { get; set; }
        public IList<Grade> Grades { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<SubjectGroup> SubjectGroups { get; set; }
        public Teacher Teacher { get; set; }
        public int? TeacherId { get; set; }

    }
}
