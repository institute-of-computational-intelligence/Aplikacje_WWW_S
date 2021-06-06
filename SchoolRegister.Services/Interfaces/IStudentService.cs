using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
using System.Threading.Tasks;

namespace SchoolRegister.Services.Interfaces
{
    public interface IStudentService
    {
        Task<GroupVm> AddStudentAsync(AttachDetachStudentToGroupVm addStudent);
        Task<GroupVm> RemoveStudentAsync(RemoveFromGroupVm removeStudent);
        Task<bool> RemoveStudentAsync(int studentId);
        IEnumerable<StudentVm> GetStudents(Expression<Func<Student, bool>> filterExpressions = null);
        Task<Student> GetStudentAsync(Expression<Func<Student, bool>> filterExpressions);
    }
}