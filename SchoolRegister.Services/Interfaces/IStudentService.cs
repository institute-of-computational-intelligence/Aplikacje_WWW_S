using System.Threading.Tasks;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IStudentService
    {
        Task<GroupVm> AddStudentToGroupAsync(AddStudentToGroupVm addStudentToGroup);
        Task<GroupVm> RemoveStudentFromGroupAsync(RemoveStudentFromGroupVm removeStudentFromGroup);
    }
}