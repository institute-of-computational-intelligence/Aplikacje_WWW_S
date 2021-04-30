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


namespace SchoolRegister.Services.Services
{
    public class TeacherService : BaseService, ITeacherService
    {
        public TeacherService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger, userManager)
        {
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
                    }
                    else
                        throw new UnauthorizedAccessException("Only Teacher can add grades.");
                }
                else
                    throw new ArgumentNullException("View model paramet requires all fields to be set");

                await DbContext.SaveChangesAsync();
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


                SmtpClient client = new SmtpClient()
                {
                    EnableSsl = true,
                    Credentials = CredentialCache.DefaultNetworkCredentials
                };

                MailMessage msg = new MailMessage(teacher.Email, parent.Email, sendEmailToParent.Title, sendEmailToParent.Content);
                await client.SendMailAsync(msg);
                client.Dispose();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}