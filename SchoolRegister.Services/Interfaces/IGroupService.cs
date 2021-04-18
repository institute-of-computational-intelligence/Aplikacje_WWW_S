using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
  public interface IGroupService
  {
    void DeleteGroupAsync(RemoveGroupVm deleteGroupVm);
    void AddGroupAsync(AddGroupVm addGroupVm);
  }
}