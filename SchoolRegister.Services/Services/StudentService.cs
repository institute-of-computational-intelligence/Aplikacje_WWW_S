using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class StudentService : BaseService, IStudentService
    {

        public StudentService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger) 
            : base(dbContext, mapper, logger)
        {

        }

        public async void AddStudentToGroupAsync(AddStudentToGroupVm addToGroupVm)
        {
            var student = await DbContext.Users.OfType<Student>().FirstOrDefaultAsync(u => u.Id == addToGroupVm.StudentId);

            if (student == null)
            {
                throw new ArgumentNullException($"Could not find user with id: {addToGroupVm.StudentId}");
            }

            var group = await DbContext.Groups.FirstOrDefaultAsync(g => g.Id == addToGroupVm.GroupId);

            if(group == null)
            {
                throw new ArgumentNullException($"Could not find group with id: {addToGroupVm.GroupId}");
            }

            student.GroupId = addToGroupVm.GroupId;

            DbContext.Users.Update(student);
            await DbContext.SaveChangesAsync();
        }

        public async void RemoveStudentFromGroupAsync(RemoveStudentFromGroupVm removeFromGroupVm)
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