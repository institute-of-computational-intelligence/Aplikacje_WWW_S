using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGroupService
    {
        Task <GroupVm> DeleteGroup(DeleteGroupVm deleteGroupVm);
        Task <GroupVm> AddGroup(AddGroupVm addGroupVm);

        GroupVm GetGroup (Expression<Func<Group, bool>> filterPredicate);
        IEnumerable<GroupVm> GetGroups (Expression<Func<Group, bool>> filterPredicate = null);

        StudentVm AttachStudentToGroup (AddStudentToGroupVm AddStudentToGroupVm);

        StudentVm RemoveStudentFromGroup (RemoveStudentFromGroupVm RemoveStudentFromGroupVm);

        GroupVm AttachSubjectToGroup (AttachDetachSubjectGroupVm attachSubjectGroupVm);
        GroupVm DetachSubjectFromGroup (AttachDetachSubjectGroupVm detachDetachSubjectVm);

    }
}
