using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IStudentService
    {
        void AddStudentAsync(AddStudentVm addStudent);
        void RemoveStudentAsync(RemoveStudentVm removeStudent);
    }
}