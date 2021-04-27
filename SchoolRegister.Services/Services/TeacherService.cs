using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolRegister.Model.DataModels;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Services
{
    public class TeacherService : BaseService, ITeacherService
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;

        public TeacherService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager, IConfiguration configuration) : base(dbContext, mapper, logger)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public async Task<Grade> AddGradeAsync(AddGradeAsyncVm addGradeToStudentVm)
        {
            try
            {
                var teacher = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == addGradeToStudentVm.TeacherId);

                if (teacher is null)
                    throw new ArgumentNullException("Nie znaleziono nauczyciela");

                if (!await userManager.IsInRoleAsync(teacher, "Teacher"))
                    throw new UnauthorizedAccessException($"Nie masz praw dostępu");

                Student student = await DbContext.Users
                    .OfType<Student>()
                    .FirstOrDefaultAsync(u => u.Id == addGradeToStudentVm.StudentId);

                if (student is null)
                    throw new ArgumentNullException("Nie znaleziono studenta");

                if (!DbContext.Subjects.Any(x => x.Id == addGradeToStudentVm.SubjectId))
                    throw new ArgumentNullException($"Nie znaleziono przedmiotu");


                Grade grade = new Grade()
                {
                    StudentId = addGradeToStudentVm.StudentId,
                    SubjectId = addGradeToStudentVm.SubjectId,
                    GradeValue = addGradeToStudentVm.GradeValue,
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

        public async void SendEmailToParent(SendEmailVm SendEmailVm)
        {
            try
            {
                User sender = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == SendEmailVm.SenderId);

                if (sender is null)
                    throw new ArgumentNullException("Nie znaleziono nadawcy");

                User recipient = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == SendEmailVm.RecipientId);

                if (recipient is null)
                    throw new ArgumentNullException("Nie znaleziono odbiorcy");

                if (!(await userManager.IsInRoleAsync(sender, "Teacher") && await userManager.IsInRoleAsync(recipient, "Parent")))
                    throw new UnauthorizedAccessException("Nie masz praw dostępu");


                if (await userManager.IsInRoleAsync(sender, "Teacher") && await userManager.IsInRoleAsync(recipient, "Parent"))
                {
                    var message = new MailMessage(sender.Email, recipient.Email, SendEmailVm.EmailSubject, SendEmailVm.EmailBody);
                    
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
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}