using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
    public class DisplayGradeVm
    {

        public int StudentId { get; set; }

        public string StudentName { get; set; }

        public string TeacherName { get; set; }

        public string SubjectName { get; set; }

        public int GradeValue { get; set; }

        public DateTime DateOfIssue { get; set; }

    }
}