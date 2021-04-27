using System.Threading.Tasks;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGroupService
    {
        Task<GroupVm> DeleteGroupAsync(DeleteGroupVm deleteGroupVm);
        Task<GroupVm> AddGroupAsync(AddGroupVm addGroupVm);
    }
}