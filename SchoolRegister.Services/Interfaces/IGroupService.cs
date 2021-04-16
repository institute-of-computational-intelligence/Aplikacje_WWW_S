using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGroupService
    {
        void AddGroupAsync(AddGroupVm addGroupVm);
        void DeleteGroupAsync(DeleteGroupVm deleteGroupVm);
    }
}