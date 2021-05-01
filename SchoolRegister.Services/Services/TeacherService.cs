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

namespace SchoolRegister.Services.Services
{
    public class TeacherService : BaseService, ITeacherService
    {
        public TeacherService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger, userManager)
        {

        }


        public async Task<Grade> AddGrade(AddGradeVm addGradeVm)
        {
            try
            {
                Grade gr=null;
                if (addGradeVm == null)
                    throw new ArgumentNullException("View model parametr is null");

                var teacher = DbContext.Users.OfType<Teacher>().FirstOrDefault(t => t.Id == addGradeVm.TeacherId);

                if (teacher == null)
                    throw new ArgumentNullException("Can't find teacher");

                if (await UserManager.IsInRoleAsync(teacher, "Teacher"))
                {
                    gr = new Grade()
                    {
                        DateOfIssue = DateTime.Now,
                        GradeValue = addGradeVm.GradeValue,
                        StudentId = addGradeVm.StudentId,
                        SubjectId = addGradeVm.SubjectId
                    };
                    await DbContext.Grades.AddAsync(gr);
                    await DbContext.SaveChangesAsync();
                }

                return gr;

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }


        public async void SendEMail(SendEMailVm sendEMailVm)
        {
            try
            {
                if(sendEMailVm is null)
                    throw new ArgumentNullException("View model parametr is null"); 

                var teacher = DbContext.Users.OfType<Teacher>().FirstOrDefault(t => t.Id == sendEMailVm.TeacherId);
                var parent = DbContext.Users.OfType<Parent>().FirstOrDefault(t => t.Id == sendEMailVm.ParentId);
                if (teacher == null)
                    throw new ArgumentNullException("Can't find teacher");
                    
                if (await UserManager.IsInRoleAsync(teacher, "Teacher"))
                {
                    SmtpClient client = new SmtpClient();

                    MailAddress to = new MailAddress(parent.Email);
                    MailAddress from = new MailAddress(teacher.Email);

                    MailMessage message = new MailMessage(from, to);
                    message.Body = sendEMailVm.Content;
                    message.Subject=sendEMailVm.Title;
                    await client.SendMailAsync(message);
                    client.Dispose();
                }
                else
                {
                    throw new UnauthorizedAccessException("You dont have permision to send e-mail's");
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