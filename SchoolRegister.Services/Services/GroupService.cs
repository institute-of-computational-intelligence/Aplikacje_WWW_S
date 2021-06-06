using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System.Threading.Tasks;

namespace SchoolRegister.Services.Services
{
    public class GroupService : BaseService, IGroupService
    {
        private readonly UserManager<User> _userManager;
        public GroupService (ApplicationDbContext dbContext,
            IMapper mapper,
            ILogger logger,
            UserManager<User> userManager) : base (dbContext, mapper, logger) {
            _userManager = userManager;
        }   

        public async Task<GroupVm> AddGroupAsync(AddUpdateGroupVm AddUpdateGroupVm)
        {
            try
            {

                Group group = await DbContext.Groups.FirstOrDefaultAsync(g => g.Name == AddUpdateGroupVm.Name);

                if (!(group is null))
                    throw new DuplicateNameException($"Group with name: {AddUpdateGroupVm.Name} already exists");

                Group newGroup = new Group() { Name = AddUpdateGroupVm.Name };
                var groupVm = Mapper.Map<GroupVm>(newGroup);
                await DbContext.Groups.AddAsync(newGroup);
                await DbContext.SaveChangesAsync();
                return groupVm;
            }
            catch(Exception ex) 
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }      
        }

        public async Task<GroupVm> DeleteGroupAsync(DeleteGroupVm deleteGroupVm)
        {
            try
            {
                var groupToBeDeleted = await DbContext.Groups.FirstOrDefaultAsync(x => x.Id == deleteGroupVm.Id);    

                var groupVM=Mapper.Map<GroupVm>(groupToBeDeleted);
                if(groupToBeDeleted == null)
                {
                    throw new ArgumentNullException($"Could not find group with id: {deleteGroupVm.Id}");
                }

                DbContext.Groups.Remove(groupToBeDeleted);
                await DbContext.SaveChangesAsync();
                return groupVM;
            }
            catch(Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public IEnumerable<GroupVm> GetGroups(Expression<Func<Group, bool>> filterExpressions = null)
        {
            try
            {
                var groupEntities = DbContext.Groups.AsQueryable();

                if (!(filterExpressions is null))
                    groupEntities = groupEntities.Where(filterExpressions);
                var groupVms = Mapper.Map<IEnumerable<GroupVm>>(groupEntities);
                return groupVms;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public GroupVm AddOrUpdateGroup (AddUpdateGroupVm addupdateGroupVm) 
        {
            if (addupdateGroupVm == null) 
            {
                throw new ArgumentNullException ($"Vm of type is null");
            }
            var groupEntity = Mapper.Map<Group> (addupdateGroupVm);
            if (addupdateGroupVm.Id == null || addupdateGroupVm.Id == 0) {
                DbContext.Groups.Add (groupEntity);
            } 
            else 
            {
                DbContext.Groups.Update (groupEntity);
            }
            DbContext.SaveChanges ();
            var groupVm = Mapper.Map<GroupVm> (groupEntity);
            return groupVm;
        }
            public StudentVm AttachStudentToGroup (AttachDetachStudentToGroupVm attachStudentToGroupVm) 
            {
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
        public GroupVm AttachSubjectToGroup (AttachDetachSubjectGroupVm attachSubjectGroup) 
        {
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
    }
} 