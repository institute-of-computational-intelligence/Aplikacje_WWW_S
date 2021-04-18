using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;

namespace SchoolRegister.Services.Services
{
    public class StudentService : BaseService, IStudentService
    {
        public StudentService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger, userManager)
        {

        }

        public void AddStudentToGroup(StudentVm studentVm)
        {
            try
            {
                if (studentVm == null)
                    throw new ArgumentNullException("View model parametr is null");

                var student = DbContext.Users.OfType<Student>().FirstOrDefault(t => t.Id == studentVm.StudentId);

                var group = DbContext.Groups.OfType<Group>().FirstOrDefault(t => t.Id == studentVm.GroupId);

                if (student == null || group == null)
                    throw new ArgumentNullException("User or group doesn't exist");

                if (group.Students.Any(s => s.Id == studentVm.StudentId))
                    throw new ArgumentException("This student already has group");
                student.GroupId = studentVm.GroupId;
                DbContext.Users.Update(student);
                group.Students.Add(student);
                DbContext.Groups.Update(group);
                DbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public void RemoveStudentFromGroup(StudentVm studentVm)
        {
            try
            {
                if (studentVm == null)
                    throw new ArgumentNullException("View model parametr is null");

                var student = DbContext.Users.OfType<Student>().FirstOrDefault(t => t.Id == studentVm.StudentId);

                var group = DbContext.Groups.OfType<Group>().FirstOrDefault(t => t.Id == studentVm.GroupId);

                if (student == null || group == null)
                    throw new ArgumentNullException("User or group doesn't exist");

                if (group.Students.Any(s => s.Id == studentVm.StudentId))
                    throw new ArgumentException("This student already has group");
                student.GroupId = studentVm.GroupId;
                DbContext.Users.Update(student);
                group.Students.Add(student);
                DbContext.Groups.Update(group);
                DbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }


    }
}