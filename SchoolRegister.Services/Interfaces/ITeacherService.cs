using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface ITeacherService
    {
        void SendEmailAsync(SendEmailVm SendEmailVm);
        void AddGradeToStudentAsync(AddGradeToStudentVm addGradeToStudenVm);
    }
}