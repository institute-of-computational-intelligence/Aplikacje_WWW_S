using System.Threading.Tasks;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface ITeacherService
    {
        void SendEmailAsync(SendEmailVm SendEmailVm);
        Task<Grade> AddGradeToStudentAsync(AddGradeToStudentVm addGradeToStudenVm);
    }
}