using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IStudentService
    {
        void AddStudentToGroupAsync(AddStudentToGroupVm addStudentToGroupVm);
        void RemoveStudentFromGroupAsync(RemoveStudentFromGroupVm removeStudentFromGroupVm);
    }
}