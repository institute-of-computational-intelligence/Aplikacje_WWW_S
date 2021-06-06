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
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace SchoolRegister.Services.Services
{
    public class TeacherService : BaseService, ITeacherService
    {
        private readonly UserManager<User> userType;
        public TeacherService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger) { userType = userManager; }

        public async void AddGradeToStudent(AddGradeToStudentVm addGradeToStudentVm)
        {
            try
            {
                if (addGradeToStudentVm == null)
                    throw new ArgumentNullException($"View model parameter is null");

                var teacher = DbContext.Users.OfType<Teacher>().FirstOrDefault(t => t.Id == addGradeToStudentVm.TeacherId);
                if (await userType.IsInRoleAsync(teacher, "Teacher"))
                {
                    if (addGradeToStudentVm.TeacherId.HasValue && addGradeToStudentVm.StudentId == 0)
                    {
                        var student = DbContext.Users.OfType<Student>().FirstOrDefault(t => t.Id == addGradeToStudentVm.StudentId);
                        await DbContext.Grades.AddAsync(new Grade()
                        { DateOfIssue = DateTime.Now, GradeValue = addGradeToStudentVm.Grade, StudentId = addGradeToStudentVm.StudentId, SubjectId = addGradeToStudentVm.StudentId });
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

        public void AddOrUpdateGrade(AddGradeToStudentVm addOrUpdateGradeVm)
        {
            throw new NotImplementedException();
        }

        public string GetTeacherGroups(TeacherVm teacherVm)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TeacherVm> GetTeachers(Expression<Func<Teacher, bool>> filterPredicate = null)
        {
            throw new NotImplementedException();
        }

        public async void SendMailToStudentParent(SendMailVm sendMailToStudentParent)
        {
            try
            {
                if (sendMailToStudentParent == null)
                    throw new ArgumentNullException($"View model paramaeter is null");
                var teacher = DbContext.Users.OfType<Teacher>().FirstOrDefault(t => t.Id == sendMailToStudentParent.TeacherId);
                var parent = DbContext.Users.OfType<Parent>().FirstOrDefault(p => p.Id == sendMailToStudentParent.ParentId);
                if (await userType.IsInRoleAsync(teacher, "Teacher") && await userType.IsInRoleAsync(parent, "Parent"))
                {
                    if ((sendMailToStudentParent.TeacherId.HasValue) || (sendMailToStudentParent.ParentId.HasValue))
                    {
                        SmtpClient client = new SmtpClient()
                        {
                            EnableSsl = true,
                            Credentials = CredentialCache.DefaultNetworkCredentials
                        };
                        await client.SendMailAsync(teacher.Email, parent.Email, sendMailToStudentParent.MailTitle, sendMailToStudentParent.MailContent);
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

        Task ITeacherService.AddGradeToStudent(AddGradeToStudentVm newGrade)
        {
            throw new NotImplementedException();
        }


       
    }
}