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
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Linq.Expressions;

namespace SchoolRegister.Services.Services
{
    public class TeacherService : BaseService, ITeacherService
    {
        private SmtpClient _smtpClient;
        public TeacherService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager, SmtpClient smtpClient) : base(dbContext, mapper, logger, userManager)
        {
            _smtpClient = smtpClient;
        }

        public async Task<Grade> AddGradeToStudent(AddGradeToStudentVm addGradeToStudentVm)
        {
            try
            {
                Grade grade = null;
                if (addGradeToStudentVm is null)
                    throw new ArgumentNullException("View model parametr is missing");

                if (addGradeToStudentVm.TeacherId != 0 || addGradeToStudentVm.StudentId != 0)
                {
                    var teacher = DbContext.Users.OfType<Teacher>().FirstOrDefault(t => t.Id == addGradeToStudentVm.TeacherId);

                    if (teacher is null)
                        throw new ArgumentNullException("Could not find Teacher with specified Id.");

                    var subject = DbContext.Subjects.FirstOrDefault(s => s.Id == addGradeToStudentVm.SubjectId);
                    if (subject is null)
                        throw new ArgumentNullException("Subject is null");

                    if (subject.TeacherId != addGradeToStudentVm.TeacherId)
                        throw new UnauthorizedAccessException("This teacher cannot add grades to specified subject");

                    if (await UserManager.IsInRoleAsync(teacher, "Teacher"))
                    {
                        grade = new Grade()
                        {
                            DateOfIssue = DateTime.Now,
                            GradeValue = addGradeToStudentVm.GradeValue,
                            StudentId = addGradeToStudentVm.StudentId,
                            SubjectId = addGradeToStudentVm.SubjectId
                        };

                        await DbContext.Grades.AddAsync(grade);
                        await DbContext.SaveChangesAsync();

                    }
                    else
                        throw new UnauthorizedAccessException("Only Teacher can add grades.");
                }
                else
                    throw new ArgumentNullException("View model paramet requires all fields to be set");

                return grade;

            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                throw;
            }
        }

        public async void SendEmailToParent(SendEmailToParentVm sendEmailToParent)
        {
            try
            {
                if (sendEmailToParent is null)
                    throw new ArgumentNullException("View model parametr is missing");

                var teacher = DbContext.Users.OfType<Teacher>().FirstOrDefault(t => t.Id == sendEmailToParent.TeacherId);
                var parent = DbContext.Users.OfType<Parent>().FirstOrDefault(p => p.Id == sendEmailToParent.ParentId);

                if (teacher is null || parent is null)
                    throw new ArgumentNullException("Could not find teacher or user with specified Id");




                if (!(await UserManager.IsInRoleAsync(teacher, "Teacher") && await UserManager.IsInRoleAsync(parent, "Parent")))
                    throw new UnauthorizedAccessException("Access denied. Only user with role Teacher privilege to call this method");


                MailMessage msg = new MailMessage(teacher.Email, parent.Email, sendEmailToParent.Title, sendEmailToParent.Content);
                await _smtpClient.SendMailAsync(msg);
                _smtpClient.Dispose();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                throw;
            }
        }

        public IEnumerable<TeacherVm> GetTeachers(Expression<Func<Teacher, bool>> filterExpression = null)
        {
            try
            {
                var teacherEntities = DbContext.Users.OfType<Teacher>().AsQueryable();
                if (filterExpression != null)
                    teacherEntities = teacherEntities.Where(filterExpression);

                var teacherVms = Mapper.Map<IEnumerable<TeacherVm>>(teacherEntities);

                return teacherVms;
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                throw;
            }
        }

    }
}