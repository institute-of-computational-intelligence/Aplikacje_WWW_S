using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IStudentService
    {
        void AddStudentToGroupAsync(AddStudentToGroupVm addStudentToGroup);
        void RemoveStudentFromGroupAsync(RemoveStudentFromGroupVm removeStudentFromGroup);
    }
}