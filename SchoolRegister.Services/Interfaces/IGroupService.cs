using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGroupService
    {
        void AddRemoveGroup(GroupVm groupVm);
        GroupVm GetGroup(Expression<Func<Group,bool>> filterExpression);
        IEnumerable<GroupVm> GetGroups(Expression<Func<Group,bool>> filtereExpression = null);
    }
}