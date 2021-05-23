using System.Threading.Tasks;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces {
    public interface IEmailSenderService {
           Task SendEmailAsync (string to, string from, string subject, string message);  
            Task<bool> SendEmailTeacher(EmailRequestVm emailRequestVm);
    }
        
}    