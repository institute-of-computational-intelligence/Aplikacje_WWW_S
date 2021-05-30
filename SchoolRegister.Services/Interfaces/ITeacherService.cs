using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace SchoolRegister.Services.Interfaces
{
    public interface ITeacherService
    {
       
          
        Task<Grade> AddGrade(AddGradeVm addGradeVm);  

        Task<bool>  SendEmailToParentAsync(SendEmailVm sendEmailTVm);
        IEnumerable<TeacherVm> GetTeachers(Expression<Func<Teacher, bool>> filterPredicate = null);
        TeacherVm GetTeacher(Expression<Func<Teacher, bool>> filterPredicate);
        IEnumerable<GroupVm> GetTeachersGroups(TeachersGroupsVm getTeachersGroups);
        
        Task<TeacherVm> GetTeacherAsync(Expression<Func<Teacher, bool>> filterExpressions);
    }
  
}   