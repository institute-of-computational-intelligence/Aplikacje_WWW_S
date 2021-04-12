using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGroupService
    {
         void DeleteGroupAsync(DeleteGroupVm deleteGroupVm);
         void AddGroupAsync(AddGroupVm addGroupVm);
    }
}