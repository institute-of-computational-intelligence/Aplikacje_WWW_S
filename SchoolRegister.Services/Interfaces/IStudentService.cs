using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IStudentService
    {
        void AddStudentToGroupAsync(AddStudentToGroupVm addToGroup);
        void RemoveStudentFromGroupAsync(RemoveStudentFromGroupVm removeFromGroup);
    }
}