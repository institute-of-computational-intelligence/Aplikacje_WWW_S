using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
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

    public class StudentService : BaseService, IStudentService
    {
        //private readonly UserManager<User> userManager;

        public StudentService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger){}

        public async Task<GroupVm> AddToGroupAsync(AddToGroupVm addToGroupVm)
        {
            var student = await DbContext.Users.OfType<Student>().FirstOrDefaultAsync(u => u.Id == addToGroupVm.StudentId);

            if (student == null)
                throw new ArgumentNullException($"Could not find user with id: {addToGroupVm.StudentId}");

            var group = await DbContext.Groups.FirstOrDefaultAsync(g => g.Id == addToGroupVm.GroupId);

            if(group == null)
                throw new ArgumentNullException($"Could not find group with id: {addToGroupVm.GroupId}");
            
            if (group.Students.Any(x => x.Id == student.Id))
                throw new DuplicateNameException($"Group with id: {addToGroupVm.GroupId} already contains student with id: ${addToGroupVm.StudentId}");

            student.GroupId = addToGroupVm.GroupId;
            group.Students.Add(student);

            DbContext.Groups.Update(group);
            DbContext.Users.Update(student);
            await DbContext.SaveChangesAsync();

            var groupVm = Mapper.Map<GroupVm>(group);
            return groupVm;
        }

        public async Task<GroupVm> RemoveFromGroupAsync(RemoveFromGroupVm removeFromGroupVm)
        {
            var student = await DbContext.Users.OfType<Student>().FirstOrDefaultAsync(u => u.Id == removeFromGroupVm.StudentId);

            if (student == null)
            {
                throw new ArgumentNullException($"Could not find user with id: {removeFromGroupVm.StudentId}");
            }

            Group group = await DbContext.Groups.FirstOrDefaultAsync(g => g.Id == removeFromGroupVm.GroupId);

            if (group is null)
                throw new ArgumentNullException($"Group with id: {removeFromGroupVm.StudentId} does not exist");

            var groupVm = Mapper.Map<GroupVm>(group);

            if (!group.Students.Remove(student))
                return null;

            student.GroupId = null;
            DbContext.Groups.Update(group);
            DbContext.Users.Update(student);
            await DbContext.SaveChangesAsync();

            return groupVm;
        }
    }
} 