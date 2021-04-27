using System;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;

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

        public async Task<Grade> AddGradeAsync(AddGradeAsyncVm addGradeVm)
        {
            try
            {
                var teacher = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == addGradeVm.TeacherId);

                if(teacher == null)       
                    throw new ArgumentNullException("Could not find specified teacherId.");

                if (!await userManager.IsInRoleAsync(teacher, "Teacher"))
                    throw new UnauthorizedAccessException($"User with id {addGradeVm.StudentId} does not have required permissions to add grade to the student");

                Student student = await DbContext.Users
                    .OfType<Student>()
                    .FirstOrDefaultAsync(u => u.Id == addGradeVm.StudentId);

                if (student is null)
                    throw new ArgumentNullException($"Student with id: {addGradeVm.StudentId} does not exist");

                if (!DbContext.Subjects.Any(x => x.Id == addGradeVm.SubjectId))
                    throw new ArgumentNullException($"Subject with id: {addGradeVm.SubjectId} does not exist");


                Grade grade = new Grade()
                {
                    StudentId = addGradeVm.StudentId,
                    SubjectId = addGradeVm.SubjectId,
                    GradeValue = addGradeVm.GradeValue,
                    DateOfIssue = DateTime.Now

                };

                await DbContext.Grades.AddAsync(grade);
                await DbContext.SaveChangesAsync();

                return grade;
            }
            catch(Exception exception)
            {
                Logger.LogError(exception.Message);
                throw;
            }
        }

        public async void SendEmailToParent(SendEmailVm sendEmailVm)
        {
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

                string sendEmailsFrom = "emailAddress@mydomain.com";             
                string sendEmailsFromPassword = "password";
                NetworkCredential credentials = new NetworkCredential(sendEmailsFrom, sendEmailsFromPassword);

                var client = new SmtpClient("smtp.gmail.com", 587);

                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Timeout = 20000;
                client.Credentials = credentials;

                client.Send(message); 
            }
        }
    }
} 