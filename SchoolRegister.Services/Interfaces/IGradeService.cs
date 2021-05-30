using SchoolRegister.ViewModels.VM;
using System.Collections.Generic;
using SchoolRegister.Model.DataModels;
using System.Threading.Tasks;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGradeService
    {
        Task<IEnumerable<Grade>> GetGrades(CheckGradesVm checkGradesVm);
    }
}