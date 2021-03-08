using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zadanie_1.Models
{
    public class Teacher : User
    {
        public IList<Subject> Subjects { get; set; }
        public string Title { get; set; }
    }
}
