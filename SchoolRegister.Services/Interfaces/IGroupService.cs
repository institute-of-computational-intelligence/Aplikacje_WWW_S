using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGroupService
    {
        Task<GroupVm> DeleteGroupAsync(DeleteGroupVm deleteGroupVm);
        Task<GroupVm> AddGroupAsync(AddGroupVm addGroupVm);
        IEnumerable<GroupVm> ShowGroups(Expression<Func<Group, bool>> filterExpressions = null);
    }
}