using System.Threading.Tasks;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IStudentService
    {
        Task<GroupVm> AddStudentAsync(AddToGroupVm addStudent);
        Task<GroupVm> RemoveStudentAsync(RemoveFromGroupVm removeStudent);
    }
}