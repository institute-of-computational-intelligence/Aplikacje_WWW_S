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
        public virtual Group Group  {get; set;}
        [ForeignKey("Group")]
        public int? GroupId  {get; set;}

        public virtual Parent Parent {get; set;}
        [ForeignKey("Parent")]
        public int? ParentId {get; set;}
       
        public virtual IList<Grade> Grades {get; set;}

        [NotMapped]
        public double AvarageGrade  {get;}
        [NotMapped]
        public IDictionary<string, double> AvarageGradePerSubject  { get;}
        
        public IDictionary<String, List<GradeScale>> GradesPerSubject  {get;}
        
    }

}