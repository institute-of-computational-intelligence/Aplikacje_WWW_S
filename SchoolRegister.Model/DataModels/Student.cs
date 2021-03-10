using Microsoft.AspNetCore.Identity;
using System;

namespace SchoolRegister.Model.DataModels
{
    public class Student : User
    {
        public double avarageGrade  {get;}
        public IDictionary<string, double> avarageGradePerSubject  { get;}
        public IList<Grade> grades {get; set;}
        public IDictionary<String, List<GradeScale>> gradesPerSubject  {get;}
        public Group group  {get; set;}
        public int? groupId  {get; set;}
        public Parent parent {get; set;}
        public int? parentId {get; set;}
    }

}