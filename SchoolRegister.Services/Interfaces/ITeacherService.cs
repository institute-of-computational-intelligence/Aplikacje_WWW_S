using System.Threading.Tasks;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface ITeacherService
    {
        void SendEmailToParent(SendEmailVm sendEmailVm);
        Task<Grade> AddGradeAsync(AddGradeAsyncVm addGradeVm);
    }
}