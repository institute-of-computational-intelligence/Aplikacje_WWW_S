using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;


namespace SchoolRegister.Services.Interfaces
{
    public interface IStudentService
    {
        Task <GroupVm> AddStudentToGroupAsync(AddOrRemStudentGroupVm addStudentToGroup);
          
     
        Task <GroupVm> RemoveStudentFromGroupAsync(AddOrRemStudentGroupVm removeStudentFromGroup);
   
        IEnumerable<StudentVm> GetStudents(Expression<Func<Student, bool>> filterPredicate = null);
        StudentVm GetStudent(Expression<Func<Student, bool>> filterPredicate);
   
        Task<Student> GetStudentAsync(Expression<Func<Student, bool>> filterExpressions);
        
    }
}