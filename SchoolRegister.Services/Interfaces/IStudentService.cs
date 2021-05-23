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
        Task<GroupVm> AddStudentToGroup(AddStudentToGroupVm addStudentToGroup);
        Task<GroupVm> RemoveStudentFromGroup(RemoveStudentFromGroupVm removeStudentFromGroup);
        Task<bool> RemoveStudent(int studentId);
        IEnumerable<StudentVm> GetStudents(Expression<Func<Student, bool>> filterExpressions = null);
        Task<Student> GetStudent(Expression<Func<Student, bool>> filterExpressions);
    }
}