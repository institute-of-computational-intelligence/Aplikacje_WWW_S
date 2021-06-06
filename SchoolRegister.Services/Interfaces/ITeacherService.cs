using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
using System.Threading.Tasks;

namespace SchoolRegister.Services.Interfaces
{

    public interface ITeacherService
    {
        Task<Grade> AddGradeAsync(AddGradeAsyncVm addGradeVm);
        IEnumerable<TeacherVm> GetTeachers(Expression<Func<Teacher, bool>> filterExpressions = null);
        Task<bool>  SendEmailToParentAsync(SendEmailVm sendEmailTVm);
    }
}