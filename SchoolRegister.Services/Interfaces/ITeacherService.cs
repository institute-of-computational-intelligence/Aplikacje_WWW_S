using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface ITeacherService
    {
        void SendEmailToParent(SendEmailVm SendEmailVm);
        Task<Grade> AddGradeAsync(AddGradeAsyncVm addGradeToStudenVm);
        IEnumerable<TeacherVm> ShowTeachers(Expression<Func<Teacher, bool>> filterExpressions = null);
    }
}