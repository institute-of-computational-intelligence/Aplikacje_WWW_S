using System;
using System.Collections.Generic;
using System.Text;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.ViewModels.VM
{
   public class GradeVm
    {

        public string Student { get; set; }
        public string Subject { get; set; }
        public DateTime DateOfIssue { get; set; }
        public GradeScale GradeValue { get; set; }
    }
}          