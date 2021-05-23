using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
using System.Threading.Tasks;

namespace SchoolRegister.Services.Interfaces
{
    public interface ITeacherService
    {
        Task<Grade> AddGradeToStudent(AddGradeToStudentVm addGradeToStudent);
        void SendMailToStudentParent(SendMailToStudentParentVm sendMailToStudentParent);
        IEnumerable<TeacherVm> GetTeachers(Expression<Func<Teacher, bool>> filterPredicate = null);
        TeacherVm GetTeacher(Expression<Func<Teacher, bool>> filterPredicate);
        IEnumerable<GroupVm> GetTeacherGroups(TeacherVm getTeachersGroups);
    }
}