using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IStudentService
    {
        void AddOrRemoveStudent(StudentVm studentVm);
    }
} 