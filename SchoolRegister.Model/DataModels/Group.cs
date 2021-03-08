using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zadanie_1.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Student> Students { get; set; }
        public IList<SubjectGroup> SubjectGroups { get; set; }
    }
}
