using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SchoolRegister.BLL.DataModels;
using SchoolRegister.ViewModels.VM;


namespace SchoolRegister.Services.Interfaces
{
    public interface IGroupService
    {
        Task<GroupVm> DeleteGroupAsync(RemoveGroupVm deleteGroupVm);
        Task<GroupVm> AddGroupAsync(AddGroupVm addGroupVm);
        IEnumerable<GroupVm> GetGroups(Expression<Func<Group, bool>> filterExpressions = null);
    }
}