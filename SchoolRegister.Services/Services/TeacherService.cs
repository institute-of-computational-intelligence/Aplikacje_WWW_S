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
    public class TeacherService : BaseService, ITeacherService
    {
        private readonly UserManager<User> userType;
        public TeacherService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger)
        {
            userType = userManager;
        }

        public async void AddGrade(AddGradeToStudentVm addGradeVm)
        {
            try
            {
                if (addGradeVm == null)
                    throw new ArgumentNullException($"View model parameter is null");

                var teacher = DbContext.Users.OfType<Teacher>().FirstOrDefault(t => t.Id == addGradeVm.TeacherId);
                if (await userType.IsInRoleAsync(teacher, "Teacher"))
                {
                    if (addGradeVm.TeacherId.HasValue && addGradeVm.StudentId == 0)
                    {
                        var student = DbContext.Users.OfType<Student>().FirstOrDefault(t => t.Id == addGradeVm.StudentId);
                        await DbContext.Grades.AddAsync(new Grade()
                        { DateOfIssue = DateTime.Now, GradeValue = addGradeVm.GradeValue, StudentId = addGradeVm.StudentId, SubjectId = addGradeVm.StudentId });
                        await DbContext.SaveChangesAsync();
                    }
                    else
                    {
                        throw new ArgumentNullException("TeacherId or StudentId have no values!");
                    }
                }
                else
                {
                    throw new ArgumentNullException("You don't have permission to add grades!");
                }

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async void SendEmail(MailVm mailVm)
        {
            try
            {
                if (mailVm == null)
                    throw new ArgumentNullException($"View model paramaeter is null");
                var teacher = DbContext.Users.OfType<Teacher>().FirstOrDefault(t => t.Id == mailVm.TeacherId);
                var parent = DbContext.Users.OfType<Parent>().FirstOrDefault(p => p.Id == mailVm.UserId);
                if (await userType.IsInRoleAsync(teacher, "Teacher") && await userType.IsInRoleAsync(parent, "Parent"))
                {
                    if ((mailVm.TeacherId.HasValue) || (mailVm.UserId.HasValue))
                    {
                        SmtpClient client = new SmtpClient()
                        {
                            EnableSsl = true,
                            Credentials = CredentialCache.DefaultNetworkCredentials
                        };
                        await client.SendMailAsync(teacher.Email, parent.Email, mailVm.Title, mailVm.Content);
                        client.Dispose();
                    }
                    else
                    {
                        throw new ArgumentNullException("TeacherId or ParentId have no values!");
                    }
                }
                else
                {
                    throw new ArgumentNullException("You have valid role to sending emails, or You are not sending email to vaild user type!");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
} 