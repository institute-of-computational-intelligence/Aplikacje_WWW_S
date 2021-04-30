using System.Threading.Tasks;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface ITeacherService
    {
        void SendMail(SendMailVm SendMailVm);
        Task<Grade> AddGradeAsync(AddGradeVm addGradeVm);
    }
}