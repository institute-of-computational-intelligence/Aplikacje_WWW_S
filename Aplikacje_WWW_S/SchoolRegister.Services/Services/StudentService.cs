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
        public StudentService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger) { }

        public void AddOrRemoveStudentGroup(StudentVm studentVm)
        {
            try
            {
                if (studentVm == null)
                    throw new ArgumentNullException($"View model parameter is null");
                var student = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == studentVm.StudentId);
                var group = DbContext.Groups.FirstOrDefault(g => g.Id == studentVm.GroupId);

                if (student == null)
                    throw new ArgumentNullException("Specifed student doesn't exist");
                if (group == null)
                    throw new ArgumentNullException("Specifed group doesn't exist");
                if (!group.Students.Any(s => s.Id == student.Id))
                {
                    student.GroupId = studentVm.GroupId;
                    DbContext.Update(student);
                    group.Students.Add(student);
                }
                else
                {
                    student.GroupId = null;
                    DbContext.Update(student);
                    group.Students.Remove(student);
                }
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