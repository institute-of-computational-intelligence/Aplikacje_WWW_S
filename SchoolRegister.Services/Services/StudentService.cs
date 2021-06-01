using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRegister.Services.Services
{
    public class StudentService : BaseService, IStudentService
    {
        public StudentService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger, userManager)
        {
        }

        public void RemoveStudentFromGroup(StudentVm studentVm)
        {
            try
            {
                if (studentVm is null || studentVm.StudentId == 0)
                    throw new ArgumentNullException("View model parametr is missing");

                var student = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == studentVm.StudentId);
                var group = DbContext.Groups.FirstOrDefault(g => g.Id == studentVm.GroupId);

                if (student is null)
                    throw new ArgumentNullException("Student with specified ID doesn't exist.");

                if (group is null)
                    throw new ArgumentNullException("Group with specified ID doesn't exist.");

                if (!group.Students.Any(s => s.Id == student.Id))
                    throw new InvalidOperationException("Student does not exist in specified group.");

                student.GroupId = null;
                DbContext.Users.Update(student);

                group.Students.Remove(student);
                DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                throw;
            }
        }

        public void AddStudentToGroup(StudentVm studentVm)
        {
            try
            {
                if (studentVm is null || studentVm.StudentId == 0)
                    throw new ArgumentNullException("View model parametr is missing");

                var student = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == studentVm.StudentId);
                var group = DbContext.Groups.FirstOrDefault(g => g.Id == studentVm.GroupId);

                if (student is null)
                    throw new ArgumentNullException("Student with specified ID doesn't exist.");

                if (group is null)
                    throw new ArgumentNullException("Group with specified ID doesn't exist.");

                if (group.Students.Any(s => s.Id == student.Id))
                    throw new InvalidOperationException("Student already exist in specified group.");

                student.GroupId = studentVm.GroupId;
                DbContext.Users.Update(student);

                group.Students.Add(student);
                DbContext.Groups.Update(group);

                DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}