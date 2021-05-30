using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace SchoolRegister.Services.Services
{
    public class GroupService : BaseService, IGroupService {
        private readonly UserManager<User> userManager;
        public GroupService (ApplicationDbContext dbContext,
            IMapper mapper,
            ILogger logger,
            UserManager<User> _userManager) : base (dbContext, mapper, logger) {
                userManager = _userManager;
        }

 public async Task<GroupVm> AddGroup(AddGroupVm addGroupVm)
        {
            try 
            {
                Group group = await DbContext.Groups.FirstOrDefaultAsync(g => g.Name == addGroupVm.Name);

                if (!(group is null))
                      throw new ArgumentNullException ($"View model parameter is null");
                        

                var groupToBeAdded = new Group() { Name = addGroupVm.Name };
                var groupVm = Mapper.Map<GroupVm>(groupToBeAdded);

                await DbContext.AddAsync(groupToBeAdded);
                await DbContext.SaveChangesAsync();  
                   

                return groupVm;
            }
            catch(Exception ex) 
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }      
        }


        public async Task<GroupVm> DeleteGroup(DeleteGroupVm deleteGroupVm)
        {
            try
            {
                var groupToBeDeleted = await DbContext.Groups.FirstOrDefaultAsync(g => g.Id == deleteGroupVm.Id);    

                if(groupToBeDeleted == null)
                {
                    throw new ArgumentNullException($"Could not find group with id: {deleteGroupVm.Id}");
                }
                var groupVm = Mapper.Map<GroupVm>(groupToBeDeleted);

                DbContext.Groups.Remove(groupToBeDeleted);
                await DbContext.SaveChangesAsync();
                
                return groupVm;
            }
            catch(Exception exception)
            {
                Logger.LogError(exception.Message);
                throw;
            }
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

        public StudentVm AttachStudentToGroup (AddStudentToGroupVm AddStudentToGroupVm) {
            if (AddStudentToGroupVm == null) {
                throw new ArgumentNullException ($"Vm of type is null");
            }
            var student = DbContext.Users.OfType<Student> ().FirstOrDefault (t => t.Id == AddStudentToGroupVm.StudentId);
            if (student == null || !userManager.IsInRoleAsync (student, "Student").Result) {
                throw new ArgumentNullException ($"Student is null or user is not student");
            }
            var group = DbContext.Groups.FirstOrDefault (x => x.Id == AddStudentToGroupVm.GroupId);
            if (group == null) {
                throw new ArgumentNullException ($"group is null");
            }
            student.GroupId = group.Id;
            student.Group = group;
            DbContext.SaveChanges ();
            var studentVm = Mapper.Map<StudentVm> (student);
            return studentVm;
        }


            public StudentVm RemoveStudentFromGroup (RemoveStudentFromGroupVm RemoveStudentFromGroupVm) {
            if (RemoveStudentFromGroupVm == null) {
                throw new ArgumentNullException ($"Vm of type is null");
            }
            var student = DbContext.Users.OfType<Student> ().FirstOrDefault (t => t.Id == RemoveStudentFromGroupVm.StudentId);
            if (student == null || !userManager.IsInRoleAsync (student, "Student").Result) {
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




    }
    
} 