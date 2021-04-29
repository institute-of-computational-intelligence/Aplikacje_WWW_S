using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SchoolRegister.Services.Services
{
    public class TeacherService : BaseService, ITeacherService
    {
        private readonly UserManager<User> userManager;
        public TeacherService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger)
        {
            this.userManager = userManager;
        }

        public async Task<Grade> AddGradeAsync(AddGradeVm addGradeVm)
        {
            try {
                if (addGradeVm == null)
                    throw new ArgumentNullException ($"View model parameter is null");

                var teacher = DbContext.Users.OfType<Teacher>().FirstOrDefault(t => t.Id == addGradeVm.TeacherId);
                if(teacher is null) 
                    throw new ArgumentNullException("TeacherId is incorrect");

                if(await userManager.IsInRoleAsync(teacher, "Teacher"))
                {
                    var student = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == addGradeVm.StudentId);
                    if(student is null) 
                        throw new ArgumentNullException("StudentId is incorrect");
                    Grade grade = new Grade()
                    {
                        DateOfIssue = DateTime.Now,
                        GradeValue = addGradeVm.Grade,
                        StudentId = addGradeVm.StudentId,
                        SubjectId = addGradeVm.SubjectId
                    };
                    await DbContext.Grades.AddAsync(grade);
                    await DbContext.SaveChangesAsync();
                    return grade;
                }
                else
                    throw new ArgumentNullException("Only teacher can add grades");
                
            } catch (Exception ex) {
                Logger.LogError (ex, ex.Message);
                throw;
            }
        }

        public async void SendEmail(SendEmailVm sendEmailVm)
        {
            try
            {
                if (sendEmailVm == null)
                    throw new ArgumentNullException ($"View model parameter is null");
                var sender = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == sendEmailVm.SenderId);

                if (sender is null)
                    throw new ArgumentNullException("SenderId is incorrect");

                var recipient = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == sendEmailVm.RecipientId);

                if (recipient is null)
                    throw new ArgumentNullException("RecipientId is incorrect");

                if (await userManager.IsInRoleAsync(sender, "Teacher") && await userManager.IsInRoleAsync(recipient, "Parent"))
                {
                    var message = new MailMessage(sender.Email, recipient.Email, sendEmailVm.Title, sendEmailVm.Content);
                    var client = new SmtpClient();
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Credentials = CredentialCache.DefaultNetworkCredentials;
                    client.Send(message); 
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
