using System.Threading.Tasks;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IStudentService
    {
        Task<GroupVm> AddToGroupAsync(AddToGroupVm addStudentToGroup);
        Task<GroupVm> RemoveFromGroupAsync(RemoveFromGroupVm removeStudentFromGroup);
    }
}