using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGroupService
    {
        void DeleteGroupAsync(RemoveGroupVm removeGroupVm);
        void AddGroupAsync(AddGroupVm addGroupVm);
        //GroupVm GetGroup (Expression<Func<Group, bool>> filterPredicate);
        //IEnumerable<GroupVm> GetGroups (Expression<Func<Group, bool>> filterPredicate = null);
    }
}