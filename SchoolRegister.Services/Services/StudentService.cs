using System;
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

namespace SchoolRegister.Services.Services
{
    public class StudentService : BaseService, IStudentService
    {
        //private readonly UserManager<User> userManager;

        public StudentService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger) { }

        public async void AddToGroupAsync(AddToGroupVm addToGroupVm)
        {
            var student = await DbContext.Users.OfType<Student>().FirstOrDefaultAsync(u => u.Id == addToGroupVm.StudentId);

            if (student == null)
            {
                throw new ArgumentNullException($"Could not find user with id: {addToGroupVm.StudentId}");
            }

            var group = await DbContext.Groups.FirstOrDefaultAsync(g => g.Id == addToGroupVm.GroupId);

            if (group == null)
            {
                throw new ArgumentNullException($"Could not find group with id: {addToGroupVm.GroupId}");
            }

            student.GroupId = addToGroupVm.GroupId;

            DbContext.Users.Update(student);
            await DbContext.SaveChangesAsync();
        }

        public async void RemoveFromGroupAsync(RemoveFromGroupVm removeFromGroupVm)
        {
            var student = await DbContext.Users.OfType<Student>().FirstOrDefaultAsync(u => u.Id == removeFromGroupVm.StudentId);

            if (student == null)
            {
                throw new ArgumentNullException($"Could not find user with id: {removeFromGroupVm.StudentId}");
            }

            student.GroupId = null;

            DbContext.Users.Update(student);
            await DbContext.SaveChangesAsync();
        }
    }
}