
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface ITeacherService
    {
        void SendEmailAsync(SendEmailVm sendEmailVm);
        void AddGradeToStudentAsync(AddGradeToStudentVm addGradeToStudenVm);
    }
}