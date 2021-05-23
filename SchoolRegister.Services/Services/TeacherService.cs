using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EntityFramework;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Services
{
    public class TeacherService : BaseService, ITeacherService
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<User> userManager;

        public TeacherService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger,
            UserManager<User> userManager, IConfiguration configuration) : base(dbContext, mapper, logger)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public async Task<Grade> AddGradeToStudentAsync(AddGradeToStudentVm addGradeToStudentVm)
        {
            try
            {
                if (addGradeToStudentVm is null)
                    throw new ArgumentNullException("View model parameter is null");

                var teacher = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == addGradeToStudentVm.TeacherId);

                if (teacher is null)
                    throw new ArgumentNullException($"Teacher with id: {addGradeToStudentVm.TeacherId} does not exist");

                if (!await userManager.IsInRoleAsync(teacher, "Teacher"))
                    throw new UnauthorizedAccessException(
                        $"User with id {addGradeToStudentVm.StudentId} does not have required permissions to add grade to the student");

                var student = await DbContext.Users
                    .OfType<Student>()
                    .FirstOrDefaultAsync(u => u.Id == addGradeToStudentVm.StudentId);

                if (student is null)
                    throw new ArgumentNullException($"Student with id: {addGradeToStudentVm.StudentId} does not exist");

                if (!DbContext.Subjects.Any(x => x.Id == addGradeToStudentVm.SubjectId))
                    throw new ArgumentNullException($"Subject with id: {addGradeToStudentVm.SubjectId} does not exist");


                var grade = new Grade
                {
                    StudentId = addGradeToStudentVm.StudentId,
                    SubjectId = addGradeToStudentVm.SubjectId,
                    GradeValue = addGradeToStudentVm.Grade,
                    DateOfIssue = DateTime.Now
                };

                await DbContext.Grades.AddAsync(grade);
                await DbContext.SaveChangesAsync();

                return grade;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<TeacherVm> GetTeachers(Expression<Func<Teacher, bool>> filterExpressions = null)
        {
            try
            {
                var teacherEntities = DbContext.Users.OfType<Teacher>().AsQueryable();
                if (!(filterExpressions is null))
                    teacherEntities = teacherEntities.Where(filterExpressions);

                var teacherVms = Mapper.Map<IEnumerable<TeacherVm>>(teacherEntities);

                return teacherVms;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task SendEmailAsync(SendEmailVm SendEmailVm)
        {
            try
            {
                var teacher = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == SendEmailVm.TeacherId);

                if (teacher is null)
                    throw new ArgumentNullException($"Teacher with id: {SendEmailVm.TeacherId} does not exist");

                var parent = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == SendEmailVm.ParentId);

                if (parent is null)
                    throw new ArgumentNullException($"Parent with id: {SendEmailVm.ParentId} does not exist");

                if (!(await userManager.IsInRoleAsync(teacher, "Teacher") &&
                      await userManager.IsInRoleAsync(parent, "Parent")))
                    throw new UnauthorizedAccessException(
                        "Insufficient permissions, teacher or parent does not have required permissions");


                var emailUsername = configuration.GetValue<string>("EmailSettings:Username");
                var emailPassword = configuration.GetValue<string>("EmailSettings:Password");
                var emailClientAddress = configuration.GetValue<string>("EmailSettings:ClientAddress");
                var emailClientPort = configuration.GetValue<int>("EmailSettings:ClientPort");
                var emailClientTimeout = configuration.GetValue<int>("EmailSettings:ClientTimeout");

                var accessCredentials = new NetworkCredential(emailUsername, emailPassword);
                var message = new MailMessage(teacher.Email, parent.Email, SendEmailVm.EmailSubject,
                    SendEmailVm.EmailContent);

                var client = new SmtpClient(emailClientAddress, emailClientPort);

                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Timeout = emailClientTimeout;
                client.Credentials = accessCredentials;

                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}