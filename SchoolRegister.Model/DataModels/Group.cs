using System.Collections.Generic;
using System;

namespace SchoolRegister.BLL.DataModels
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual IList<Student> Students { get; set; }
        public virtual IList<SubjectGroup> SubjectGroups { get; set; }
    }
}