using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGroupService
    {
        void DeleteGroupAsync(RemoveGroupVm removeGroupVm);
        void AddGroupAsync(AddGroupVm addGroupVm);
    }
}