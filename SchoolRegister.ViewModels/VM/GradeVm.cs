using System;
using System.Collections.Generic;
using System.Text;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.ViewModels.VM
{
   public class GradeVm
    {
        public string SubjectName { get; set; }

        public GradeScale GradeValue { get; set; }
    }
}
