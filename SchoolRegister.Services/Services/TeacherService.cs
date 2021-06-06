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
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Services
{
    public class TeacherService : BaseService, ITeacherService
    {
        private readonly UserManager<User> _userManager;
        private readonly SmtpClient _smtpClient;
        public TeacherService(UserManager<User> userManager, SmtpClient smtpClient, ApplicationDbContext dbContext, IMapper mapper, ILogger logger) 
            : base(dbContext, mapper, logger)
        {
            _userManager = userManager;
            _smtpClient = smtpClient;
        }

        public async Task<Grade> AddGradeAsync(AddGradeAsyncVm addGradeVm)
        {
            try
            {
                var teacher = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == addGradeVm.TeacherId);
                //var teacher = DbContext.Users.OfType<Teacher>().FirstOrDefault(t => t.Id== addGradeVm.TeacherId);
                if(teacher == null)
                {
                    throw new ArgumentNullException("Could not find specified TeacherId.");
                }

                if(await _userManager.IsInRoleAsync(teacher, "Teacher"))
                {
                    var student = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == addGradeVm.StudentId);

                    if (student == null)
                    {
                        throw new ArgumentNullException("Could not find specified StudentId.");
                    }

                    var grade = new Grade() { 
                        DateOfIssue = DateTime.Now, 
                        GradeValue = addGradeVm.GradeValue, 
                        StudentId = addGradeVm.StudentId, 
                        SubjectId = addGradeVm.SubjectId
                    };

                    await DbContext.Grades.AddAsync(grade);
                    await DbContext.SaveChangesAsync();
                    return grade;
                }
                else
                {
                    throw new ArgumentException("Current user does not have required permissions to performe this action.");
                }
            }
            catch(Exception ex)
            {
                Logger.LogError(ex.Message);
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

        public async Task<bool> SendEmailToParentAsync(SendEmailVm sendEmailVm)
        {
            try
            {
                if (sendEmailVm == null)
                {
                    throw new ArgumentNullException($"Vm is null");
                }

                var teacher = DbContext.Users.OfType<Teacher>()
                    .FirstOrDefault(x => x.Id == sendEmailVm.SenderId);
                if (teacher == null || _userManager.IsInRoleAsync(teacher, "Teacher").Result == false)
                {
                    throw new InvalidOperationException("sender is not teacher");
                }

                var student = DbContext.Users.OfType<Student>().FirstOrDefault(x => x.Id == sendEmailVm.StudentId);
                if (student == null || !_userManager.IsInRoleAsync(student, "Student").Result)
                {
                    throw new InvalidOperationException("given user is not student");
                }
                try
                {
                    string to = student.Parent.Email;
                    string from = teacher.Email;
                    string subject = sendEmailVm.Title;
                    string message = sendEmailVm.Content;
                    if (string.IsNullOrWhiteSpace (to) || string.IsNullOrWhiteSpace (teacher.Email) || string.IsNullOrWhiteSpace (message))
                    {
                    throw new ArgumentNullException ($"Email, subject or message is null");
                    }
                    try 
                    {
                        var mailMessage = new MailMessage (to: to,
                            subject: subject,
                            body: message,
                            from: from);
                        await _smtpClient.SendMailAsync (mailMessage);

                    } 
                    catch (Exception ex) 
                    {
                        Logger.LogError (ex, ex.Message);
                        throw;
                    }
                }
                catch (Exception ex) 
                {
                    Logger.LogError (ex, ex.Message);
                    throw;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}