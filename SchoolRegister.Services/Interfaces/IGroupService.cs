using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces {
    public interface IGroupService {
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