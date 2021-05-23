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
        void AddOrUpdateGrade(AddGradeToStudentVm addOrUpdateGradeVm);
        void SendMailToStudentParent(SendMailVm sendMailVm);
        Task AddGradeToStudent(AddGradeToStudentVm newGrade);
        IEnumerable<TeacherVm> GetTeachers(Expression<Func<Teacher, bool>> filterPredicate = null);
    }
}