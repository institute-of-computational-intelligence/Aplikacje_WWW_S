using System.Collections.Generic;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
using System.Threading.Tasks;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGradeService
    {
        Task<IEnumerable<Grade>> ViewGrades(GradesVm gradesVm);
    }
} 