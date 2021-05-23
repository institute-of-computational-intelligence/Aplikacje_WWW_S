using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.Model.DataModels;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

namespace SchoolRegister.Services.Services
{
    public class EmailSenderService : BaseService, IEmailSender, IEmailSenderService, IDisposable {
        private readonly SmtpClient _smtpClient;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        public EmailSenderService (ApplicationDbContext dbContext, ILogger logger, IMapper mapper, SmtpClient smtpClient, UserManager<User> userManager, IConfiguration configuration) : base (dbContext, mapper, logger) {
            _smtpClient = smtpClient;
            _configuration = configuration;
            _userManager = userManager;
        }
       

        public async Task SendEmailAsync (string to, string subject, string message) {
            try {
                var from = _configuration.GetValue<string> ("Email:Smtp:Username");
                await SendEmailAsync(to,subject, message, from);
            } catch (Exception ex) {
                Logger.LogError (ex, ex.Message);
                throw;      
            }
        }



        public async Task SendEmailAsync (string to, string from, string subject, string message) {
            if (string.IsNullOrWhiteSpace (to) || string.IsNullOrWhiteSpace (subject) || string.IsNullOrWhiteSpace (message))
                throw new ArgumentNullException ($"Email, subject or message is null");

            try {
                var mailMessage = new MailMessage (to: to,
                    subject: subject,
                    body: message,
                    from: from);
                await _smtpClient.SendMailAsync (mailMessage);

            } catch (Exception ex) {
                Logger.LogError (ex, ex.Message);
                throw;
            }

        }
        
        public void Dispose()
        {
            _smtpClient?.Dispose();
        }


    public async Task<bool> SendEmailTeacher(EmailRequestVm emailRequestVm)
        {
            try
            {
                if (emailRequestVm == null)
                    throw new ArgumentNullException("Email can not be null.");

                var teacher = await DbContext.Users.FirstOrDefaultAsync(t => t.Id == emailRequestVm.TeacherId);

                var parent = await DbContext.Users.FirstOrDefaultAsync(p => p.Id == emailRequestVm.ParentId);


                if (parent == null || teacher == null)
                    throw new ArgumentException($"There is no user with id {emailRequestVm.TeacherId} or {emailRequestVm.ParentId}");
                if (!await _userManager.IsInRoleAsync(teacher, "Teacher") || !await _userManager.IsInRoleAsync(parent, "Parent"))
                    throw new InvalidOperationException("Sender must be a teacher, and recipient must be a parent.");

                using (MailMessage message = new MailMessage(teacher.Email, parent.Email, emailRequestVm.EmailSubject, emailRequestVm.EmailBody))
                {
                    if (emailRequestVm.Attachments != null)
                    {
                        foreach (var attachment in emailRequestVm.Attachments)
                        {
                            string fileName = Path.GetFileName(attachment.FileName);
                            message.Attachments.Add(new Attachment(attachment.OpenReadStream(), fileName));
                        }
                    }
                    await _smtpClient.SendMailAsync(message);
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
