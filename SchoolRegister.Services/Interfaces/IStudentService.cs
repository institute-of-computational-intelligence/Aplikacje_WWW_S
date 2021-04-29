using SchoolRegister.ViewModels.VM;
  
using System.Threading.Tasks;

namespace SchoolRegister.Services.Interfaces
{
    public interface IStudentService
    {
        Task<GroupVm> AddStudentToGroupAsync(AddStudentToGroupVm addStudentToGroup);
        Task<GroupVm> RemoveStudentFromGroupAsync(RemoveStudentFromGroupVm removeStudentFromGroup);
    }
}