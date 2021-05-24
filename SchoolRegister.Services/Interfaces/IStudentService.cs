using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IStudentService
    {
        Task<GroupVm> AddToGroupAsync(AddToGroupVm addStudentToGroup);
        Task<GroupVm> RemoveFromGroupAsync(RemoveFromGroupVm removeStudentFromGroup);
        IEnumerable<StudentVm> ShowStudents(Expression<Func<Student, bool>> filterExpressions = null);
        Task<Student> ShowStudentAsync(Expression<Func<Student, bool>> filterExpressions);
    }
}