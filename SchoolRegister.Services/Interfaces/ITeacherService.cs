using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SchoolRegister.Model.DataModels;
using System.Threading.Tasks;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface ITeacherService
    {
        Task<Grade> AddGradeToStudent(AddGradeToStudentVm addGradeToStudentVm);
        void SendEmailToParent(SendEmailToParentVm sendEmailToParent);
        IEnumerable<TeacherVm> GetTeachers(Expression<Func<Teacher, bool>> filterExpression = null);
    }
}