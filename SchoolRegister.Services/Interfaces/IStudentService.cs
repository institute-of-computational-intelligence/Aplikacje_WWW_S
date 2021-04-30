using System.Threading.Tasks;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IStudentService
    {
        Task<GroupVm> AddStudentAsync(AddStudentToGroupVm addStudent);
        Task<GroupVm> RemoveStudentAsync(RemoveStudentFromGroupVm removeStudent);
    }
}