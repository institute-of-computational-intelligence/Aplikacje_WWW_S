using System;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;

namespace SchoolRegister.Services.Services
{
    public class EmailSenderService : BaseService, IEmailSender, IEmailSenderService, IDisposable {
        private readonly SmtpClient _smtpClient;
        private readonly IConfiguration _configuration;
        public EmailSenderService (ApplicationDbContext dbContext, ILogger logger, IMapper mapper, SmtpClient smtpClient, IConfiguration configuration) : base (dbContext, mapper, logger) {
            _smtpClient = smtpClient;
            _configuration = configuration;
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
    }
}