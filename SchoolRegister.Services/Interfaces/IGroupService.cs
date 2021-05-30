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
       Task <GroupVm> AddGroupAsync(AddGroupVm addGroupVm);
        
       Task <GroupVm> DeleteGroupAsync(DeleteGroupVm deleteGroupVm);
        GroupVm AddOrUpdateGroup (AddOrUpdateGroupVm addOrUpdateGroupVm);
        GroupVm GetGroup (Expression<Func<Group, bool>> filterPredicate);
        IEnumerable<GroupVm> GetGroups (Expression<Func<Group, bool>> filterPredicate = null);
        StudentVm AttachStudentToGroup (AttachDetachStudentToGroupVm attachStudentToGroupVm);
        StudentVm DetachStudentFromGroup (AttachDetachStudentToGroupVm detachStudentToGroupVm);
        GroupVm AttachSubjectToGroup (AttachDetachSubjectGroupVm attachSubjectGroupVm);
        GroupVm DetachSubjectFromGroup (AttachDetachSubjectGroupVm detachDetachSubjectVm);
        SubjectVm AttachTeacherToSubject (AttachDetachSubjectToTeacherVm attachDetachSubjectToTeacherVm);
        SubjectVm DetachTeacherFromSubject (AttachDetachSubjectToTeacherVm attachDetachSubjectToTeacherVm);
  
   } 
}
