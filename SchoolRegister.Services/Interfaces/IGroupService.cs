using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGroupService
    {
        void AddOrRemoveGroup(GroupVm groupVm);
    }
} 