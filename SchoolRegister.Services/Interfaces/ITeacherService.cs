using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface ITeacherService
    {
        void SendMail(SendMailVm SendMailVm);
        void AddGradeAsync(AddGradeVm addGradeVm);
    }
}