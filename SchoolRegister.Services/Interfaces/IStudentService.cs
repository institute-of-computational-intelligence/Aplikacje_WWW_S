using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SchoolRegister.BLL.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IStudentService
    {
        Task<GroupVm> AddStudentToGroupAsync(AddStudentToGroupVm addStudentToGroup);
        Task<GroupVm> RemoveStudentFromGroupAsync(RemoveStudentFromGroupVm removeStudentFromGroup);
        Task<bool> RemoveStudentAsync(int studentId);
        IEnumerable<StudentVm> GetStudents(Expression<Func<Student, bool>> filterExpressions = null);
        Task<Student> GetStudentAsync(Expression<Func<Student, bool>> filterExpressions);
    }
}