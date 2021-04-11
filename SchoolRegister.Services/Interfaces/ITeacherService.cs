using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface ITeacherService
    {
        void SendEmail(SendEmailVm SendEmailVm);
        void AddGradeAsync(AddGradeVm addGradeToStudenVm);
    }
}