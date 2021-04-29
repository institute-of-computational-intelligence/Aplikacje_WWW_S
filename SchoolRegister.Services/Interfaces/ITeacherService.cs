using System.Threading.Tasks;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface ITeacherService
    {
        void SendEmail(SendEmailVm SendEmailVm);
        Task<Grade> AddGradeAsync(AddGradeVm addGradeToStudenVm);
    }
}