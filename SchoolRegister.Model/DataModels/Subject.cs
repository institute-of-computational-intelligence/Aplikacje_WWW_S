using System.Collections.Generic;
using System;

namespace SchoolRegister.BLL.DataModels
{
    public class Subject
    {
        public string Description { get; set; }
        public virtual IList<Grade> Grades { get; set; }
        public int Id { get; set; }
        public String Name { get; set; }
        public virtual IList<SubjectGroup> SubjectGroups { get; set; }
        public virtual Teacher Teacher { get; set; }
        public int? TeacherId { get; set; }
    }
}