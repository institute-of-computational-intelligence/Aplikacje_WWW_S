using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zadanie_1.Models
{
    public class Parent : User
    {
        public IList<Student> Students;
    }
}
