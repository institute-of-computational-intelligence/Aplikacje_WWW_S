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
        Task<GroupVm> DeleteGroup(RemoveGroupVm deleteGroupVm);
        Task<GroupVm> AddGroup(AddGroupVm addGroupVm);
        IEnumerable<GroupVm> GetGroups(Expression<Func<Group, bool>> filterExpressions = null);
    }
}