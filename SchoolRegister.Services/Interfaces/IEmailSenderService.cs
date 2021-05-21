using System.Threading.Tasks;

namespace SchoolRegister.Services.Interfaces {
    public interface IEmailSenderService {
        Task SendEmailAsync (string to, string from, string subject, string message);
    }
}