using SchoolRegister.ViewModels.VM;
using System.Collections.Generic;
using SchoolRegister.BLL.DataModels;
using System.Threading.Tasks;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGradeService
    {
        Task<IEnumerable<Grade>> GetGradesAsync(GetGradesVm getGradesVm);
    }
}