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
        Task<GroupVm> AddStudentToGroupAsync(AddStudentToGroupVm addStudentToGroupVm);
        Task<GroupVm> RemoveStudentFromGroupAsync(RemoveStudentFromGroupVm removeStudentFromGroupVm);
        Task<bool> RemoveStudentAsync(int studentId);
        IEnumerable<StudentVm> GetStudents(Expression<Func<Student, bool>> filterExpressions = null);
        Task<Student> GetStudentAsync(Expression<Func<Student, bool>> filterExpressions);
    }
}