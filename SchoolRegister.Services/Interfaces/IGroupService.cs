using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGroupService
    {
        void AddGroupAsync(AddGroupVm addGroupVm);
        string GetGroups();
        object GetGroup(Func<object, bool> p);
        void AddRemoveGroup(GroupVm groupVm);
        object AddRemoveGroup(AddGroupVm addOrUpdateGroupVm);
        object AddGroup(AddGroupVm addOrUpdateGroupVm);
        StudentVm AttachStudentToGroup(AttachStudentGroupVm attachStudentToGroupVm);
        StudentVm DetachStudentFromGroup(AttachStudentGroupVm detachStudentToGroupVm);
    }
}