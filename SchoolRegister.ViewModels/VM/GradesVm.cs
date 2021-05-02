using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.ViewModels.VM
{
    public class GradesVm
    {
        public int UserId {get;set;}
        public int StudentId {get;set;}
        public int SubjectId {get;set;}

    }
}