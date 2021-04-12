using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface ITeacherService
    {
        void AddGradeAsync(AddGradeAsyncVm addGradeVm);
        void SendEmailToParent(SendEmailVm sendEmailVm);
    }
}