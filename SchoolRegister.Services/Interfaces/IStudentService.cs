using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IStudentService
    {
        IEnumerable<StudentVm> GetStudents(Expression<Func<Student, bool>> filterPredicate = null);
        StudentVm GetStudent(Expression<Func<Student, bool>> filterPredicate);
    }
}
