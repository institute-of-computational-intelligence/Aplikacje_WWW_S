using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.InterFaces
{
    public interface ITeacherService
    {        public void AddGrade(AddGradeVm addGradeVm);
    }
}