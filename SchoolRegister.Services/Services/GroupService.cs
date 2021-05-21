using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Services {
    public class GroupService : BaseService, IGroupService {
        private readonly UserManager<User> _userManager;
        public GroupService (ApplicationDbContext dbContext,
            IMapper mapper,
            ILogger logger,
            UserManager<User> userManager) : base (dbContext, mapper, logger) {
            _userManager = userManager;
        }

        public GroupVm AddOrUpdateGroup (AddOrUpdateGroupVm addOrUpdateGroupVm) {
            if (addOrUpdateGroupVm == null) {
                throw new ArgumentNullException ($"Vm of type is null");
            }
            var groupEntity = Mapper.Map<Group> (addOrUpdateGroupVm);
            if (addOrUpdateGroupVm.Id == null || addOrUpdateGroupVm.Id == 0) {
                DbContext.Groups.Add (groupEntity);
            } else {
                DbContext.Groups.Update (groupEntity);
            }
            DbContext.SaveChanges ();
            var groupVm = Mapper.Map<GroupVm> (groupEntity);
            return groupVm;
        }

        public GroupVm GetGroup (Expression<Func<Group, bool>> filterPredicate) {
            if (filterPredicate == null) {
                throw new ArgumentNullException ($"Predicate is null");
            }

            var groupEntity = DbContext.Groups
                .FirstOrDefault (filterPredicate);
            var groupVm = Mapper.Map<GroupVm> (groupEntity);
            return groupVm;
        }

        public IEnumerable<GroupVm> GetGroups (Expression<Func<Group, bool>> filterPredicate = null) {
            var groupEntities = DbContext.Groups.AsQueryable ();
            if (filterPredicate != null) {
                groupEntities = groupEntities.Where (filterPredicate);
            }
            var groupVms = Mapper.Map<IEnumerable<GroupVm>> (groupEntities.ToList ());
            return groupVms;
        }

        public StudentVm AttachStudentToGroup (AttachDetachStudentToGroupVm attachStudentToGroupVm) {
            if (attachStudentToGroupVm == null) {
                throw new ArgumentNullException ($"Vm of type is null");
            }
            var student = DbContext.Users.OfType<Student> ().FirstOrDefault (t => t.Id == attachStudentToGroupVm.StudentId);
            if (student == null || !_userManager.IsInRoleAsync (student, "Student").Result) {
                throw new ArgumentNullException ($"Student is null or user is not student");
            }
            var group = DbContext.Groups.FirstOrDefault (x => x.Id == attachStudentToGroupVm.GroupId);
            if (group == null) {
                throw new ArgumentNullException ($"group is null");
            }
            student.GroupId = group.Id;
            student.Group = group;
            DbContext.SaveChanges ();
            var studentVm = Mapper.Map<StudentVm> (student);
            return studentVm;
        }

        public StudentVm DetachStudentFromGroup (AttachDetachStudentToGroupVm detachStudentToGroupVm) {
            if (detachStudentToGroupVm == null) {
                throw new ArgumentNullException ($"Vm of type is null");
            }
            var student = DbContext.Users.OfType<Student> ().FirstOrDefault (t => t.Id == detachStudentToGroupVm.StudentId);
            if (student == null || !_userManager.IsInRoleAsync (student, "Student").Result) {
                throw new ArgumentNullException ($"Student is null or user is not student");
            }
            student.GroupId = null;
            student.Group = null;
            DbContext.SaveChanges ();
            var studentVm = Mapper.Map<StudentVm> (student);
            return studentVm;
        }

        public GroupVm AttachSubjectToGroup (AttachDetachSubjectGroupVm attachSubjectGroup) {
            if (attachSubjectGroup == null) {
                throw new ArgumentNullException ($"Vm of type is null");
            }
            var subjectGroup = DbContext.SubjectGroups
                .FirstOrDefault (sg => sg.GroupId == attachSubjectGroup.GroupId && sg.SubjectId == attachSubjectGroup.SubjectId);
            if (subjectGroup != null) {
                throw new ArgumentNullException ($"There is such attachment already defined.");
            }
            subjectGroup = new SubjectGroup {
                GroupId = attachSubjectGroup.GroupId,
                SubjectId = attachSubjectGroup.SubjectId
            };
            DbContext.SubjectGroups.Add (subjectGroup);
            DbContext.SaveChanges ();
            var group = DbContext.Groups.FirstOrDefault (x => x.Id == attachSubjectGroup.GroupId);
            var groupVm = Mapper.Map<GroupVm> (group);
            return groupVm;
        }

        public GroupVm DetachSubjectFromGroup (AttachDetachSubjectGroupVm detachDetachSubject) {
            if (detachDetachSubject == null) {
                throw new ArgumentNullException ($"Vm of type is null");
            }
            var subjectGroup = DbContext.SubjectGroups
                .FirstOrDefault (sg => sg.GroupId == detachDetachSubject.GroupId && sg.SubjectId == detachDetachSubject.SubjectId);
            if (subjectGroup == null) {
                throw new ArgumentNullException ($"The is no such attachment between group and subject");
            }
            DbContext.SubjectGroups.Remove (subjectGroup);
            DbContext.Remove (subjectGroup);
            DbContext.SaveChanges ();
            var group = DbContext.Groups.FirstOrDefault (x => x.Id == detachDetachSubject.GroupId);
            var groupVm = Mapper.Map<GroupVm> (group);
            return groupVm;
        }

        public SubjectVm AttachTeacherToSubject (AttachDetachSubjectToTeacherVm attachDetachSubjectToTeacherVm) {
            if (attachDetachSubjectToTeacherVm == null) {
                throw new ArgumentNullException ($"Vm of type is null");
            }
            var teacher = DbContext.Users.OfType<Teacher> ().FirstOrDefault (t => t.Id == attachDetachSubjectToTeacherVm.TeacherId);
            if (teacher == null || !_userManager.IsInRoleAsync (teacher, "Teacher").Result) {
                throw new ArgumentNullException ($"Techer is null or user is not teacher");
            }
            var subject = DbContext.Subjects.FirstOrDefault (x => x.Id == attachDetachSubjectToTeacherVm.SubjectId);
            if (subject == null) {
                throw new ArgumentNullException ($"subject is null");
            }
            subject.TeacherId = teacher.Id;
            subject.Teacher = teacher;
            DbContext.SaveChanges ();
            var subjectVm = Mapper.Map<SubjectVm> (subject);
            return subjectVm;
        }

        public SubjectVm DetachTeacherFromSubject (AttachDetachSubjectToTeacherVm attachDetachSubjectToTeacherVm) {
            if (attachDetachSubjectToTeacherVm == null) {
                throw new ArgumentNullException ($"Vm of type is null");
            }
            var subject = DbContext.Subjects.FirstOrDefault (x => x.Id == attachDetachSubjectToTeacherVm.SubjectId);
            if (subject == null) {
                throw new ArgumentNullException ($"subject is null");
            }
            subject.TeacherId = null;
            subject.Teacher = null;
            DbContext.SaveChanges ();
            var subjectVm = Mapper.Map<SubjectVm> (subject);
            return subjectVm;
        }
    }
}