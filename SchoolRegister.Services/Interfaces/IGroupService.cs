using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
using System.Threading.Tasks;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGroupService
    {
        Task<GroupVm> AddGroupAsync(AddUpdateGroupVm addupdateGroupVm);
        Task<GroupVm> DeleteGroupAsync(DeleteGroupVm deleteGroupVm);
        IEnumerable<GroupVm> GetGroups(Expression<Func<Group, bool>> filterExpressions = null);
        GroupVm AddOrUpdateGroup (AddUpdateGroupVm addupdateGroupVm);
        StudentVm AttachStudentToGroup (AttachDetachStudentToGroupVm attachStudentToGroupVm);
        StudentVm DetachStudentFromGroup (AttachDetachStudentToGroupVm detachStudentToGroupVm);
        GroupVm AttachSubjectToGroup (AttachDetachSubjectGroupVm attachSubjectGroupVm);
        GroupVm DetachSubjectFromGroup (AttachDetachSubjectGroupVm detachDetachSubjectVm);
    }
} 