using System.Threading.Tasks;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGroupService
    {
        Task<GroupVm> DeleteGroupAsync(RemoveGroupVm deleteGroupVm);
        Task<GroupVm> AddGroupAsync(AddGroupVm addGroupVm);
    }
}