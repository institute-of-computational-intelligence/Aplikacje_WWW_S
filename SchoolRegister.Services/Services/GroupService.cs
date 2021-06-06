using System;
using System.Collections.Generic;
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

namespace SchoolRegister.Services.Services
{

    public class GroupService : BaseService, IGroupService

    {
        public GroupService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger) { }

        public object AddGroup(AddGroupVm addOrUpdateGroupVm)
        {
            throw new NotImplementedException();
        }

        public async void AddGroupAsync(AddGroupVm addGroupVm)
        {
            if (string.IsNullOrEmpty(addGroupVm.Name))
            {
                throw new ArgumentNullException("Name value cannot be null or empty!");
            }

            var groupToBeAdded = new Group() { Name = addGroupVm.Name };

            await DbContext.AddAsync(groupToBeAdded);
            await DbContext.SaveChangesAsync();
        }

        public void AddRemoveGroup(GroupVm groupVm)
        {
            throw new NotImplementedException();
        }

        public object AddRemoveGroup(AddGroupVm addOrUpdateGroupVm)
        {
            throw new NotImplementedException();
        }

        public object GetGroup(Func<object, bool> p)
        {
            throw new NotImplementedException();
        }

        public string GetGroups()
        {
            throw new NotImplementedException();
        }


        public StudentVm AttachStudentToGroup(AttachStudentGroupVm attachStudentToGroupVm)
        {
            if (attachStudentToGroupVm == null)
            {
                throw new ArgumentNullException($"Vm of type is null");
            }
            var student = DbContext.Users.OfType<Student>().FirstOrDefault(t => t.Id == attachStudentToGroupVm.StudentId);
            if (student == null)
            {
                throw new ArgumentNullException($"Student is null or user is not student");
            }
            var group = DbContext.Groups.FirstOrDefault(x => x.Id == attachStudentToGroupVm.GroupId);
            if (group == null)
            {
                throw new ArgumentNullException($"group is null");
            }
            student.GroupId = group.Id;
            student.Group = group;
            DbContext.SaveChanges();
            var studentVm = Mapper.Map<StudentVm>(student);
            return studentVm;
        }

        public StudentVm DetachStudentFromGroup(AttachStudentGroupVm detachStudentToGroupVm)
        {
            if (detachStudentToGroupVm == null)
            {
                throw new ArgumentNullException($"Vm of type is null");
            }
            var student = DbContext.Users.OfType<Student>().FirstOrDefault(t => t.Id == detachStudentToGroupVm.StudentId);
            if (student == null)
            {
                throw new ArgumentNullException($"Student is null or user is not student");
            }
            student.GroupId = null;
            student.Group = null;
            DbContext.SaveChanges();
            var studentVm = Mapper.Map<StudentVm>(student);
            return studentVm;
        }
    }
}