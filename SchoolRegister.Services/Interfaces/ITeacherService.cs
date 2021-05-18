using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SchoolRegister.BLL.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface ITeacherService
    {
        void SendEmailAsync(SendEmailVm SendEmailVm);
        Task<Grade> AddGradeToStudentAsync(AddGradeToStudentVm addGradeToStudenVm);
        IEnumerable<TeacherVm> GetTeachers(Expression<Func<Teacher, bool>> filterExpressions = null);
    }
}