using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IStudentService
    {
        void AddOrRemoveStudentGroup (StudentVm studentVm);
        StudentVm GetStudent(Expression<Func<Student,bool>> filterExpression);
        IEnumerable<StudentVm> GetStudents(Expression<Func<Student,bool>> filtereExpression = null);

    }
}