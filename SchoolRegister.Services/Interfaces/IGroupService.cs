using System.Threading.Tasks;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGroupService
    {
        Task<GroupVm> AddGroupAsync(AddGroupVm addGroupVm);
        Task<GroupVm> DeleteGroupAsync(DeleteGroupVm deleteGroupVm);
    }
}