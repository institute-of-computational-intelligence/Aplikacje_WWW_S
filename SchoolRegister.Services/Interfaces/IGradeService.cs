using SchoolRegister.ViewModels.VM;
using System.Threading.Tasks;
using System.Collections.Generic;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGradeService
    {
        Task<IEnumerable<Grade>> ShowGrades(CheckGradesVm showGradesVm);
    }
}