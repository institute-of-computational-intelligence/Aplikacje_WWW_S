using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
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
        private readonly UserManager<User> userManager;

        public TeacherService(UserManager<User> userManager, ApplicationDbContext dbContext, IMapper mapper, ILogger logger) 
            : base(dbContext, mapper, logger)
        {
            this.userManager = userManager;
        }
        public async void AddGradeAsync(AddGradeAsyncVm addGradeVm)
        {
            try
            {
                var teacher = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == addGradeVm.TeacherId);
                if(teacher == null)
                {
                    throw new ArgumentNullException("Could not find specified TeacherId.");
                }

                if(await userManager.IsInRoleAsync(teacher, "Teacher"))
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
        public async void SendEmailToParent(SendEmailVm sendEmailVm)
        {
            try{
            var sender = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == sendEmailVm.SenderId);
            
            if(sender == null)
            {
                throw new ArgumentNullException("Could not find specified SenderId.");
            }
            
            var recipient = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == sendEmailVm.RecipientId);

            if(recipient == null)
            {
                throw new ArgumentNullException("Could not find specified SenderId.");
            }

            if (await userManager.IsInRoleAsync(sender, "Teacher") && await userManager.IsInRoleAsync(recipient, "Parent"))
            {
                var message = new MailMessage(sender.Email, recipient.Email, sendEmailVm.EmailSubject, sendEmailVm.EmailBody);
                
                string sendEmailsFrom = "emailAddress@gmail.com";             
                string sendEmailsFromPassword = "strongPassword";
                NetworkCredential credentials = new NetworkCredential(sendEmailsFrom, sendEmailsFromPassword);

                var client = new SmtpClient("smtp.gmail.com", 587);
                
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Timeout = 20000;
                client.Credentials = credentials;
                
                client.Send(message); 
            }
            }catch(Exception ex){
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}