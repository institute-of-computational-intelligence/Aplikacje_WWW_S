using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SchoolRegister.Services.Interfaces
{

    public interface ITeacherService
    {
        Task<bool>  SendEmailToParentAsync(SendEmailToParentVm sendEmailToParentVm);

        IEnumerable<TeacherVm> GetTeachers(Expression<Func<Teacher, bool>> filterPredicate = null);

        TeacherVm GetTeacher(Expression<Func<Teacher, bool>> filterPredicate);

        IEnumerable<GroupVm> GetTeachersGroups(TeachersGroupsVm getTeachersGroups);
    }
}