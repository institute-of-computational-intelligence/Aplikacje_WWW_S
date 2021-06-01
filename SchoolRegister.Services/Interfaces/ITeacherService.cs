using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface ITeacherService
    {
        void AddGradeAsync(AddGradeToStudentVm addGradeVm);
        void SendEmailToParent(SendEmailVm sendEmailVm);
    }
}