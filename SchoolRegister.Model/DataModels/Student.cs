using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SchoolRegister.Model.DataModels
{
    public class Student : User
    {
        public int? GroupId { get; set; }
        public virtual Group Group { get; set; } 
        [ForeignKey("Group")] 

        public virtual IList<Grade> Grades { get; set; }
       
        public int? ParentId { get; set; }
        public virtual Parent Parent { get; set; }
        [ForeignKey("Parent")]

        [NotMapped]
        public double AverageGrade { get; }
        [NotMapped]
        public IDictionary<string, double> AverageGradePerSubject { get; }
        [NotMapped]
        public IDictionary<string, List<GradeScale>> GradesPerSubject {get;}

        

    }
}
