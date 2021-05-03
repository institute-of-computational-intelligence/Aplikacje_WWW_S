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
        Task <GroupVm> AddStudentToGroup(AddStudentToGroupVm addToGroupVm);
        Task <GroupVm> RemoveStudentFromGroup(RemoveStudentFromGroupVm removeFromGroupVm);
    }
}