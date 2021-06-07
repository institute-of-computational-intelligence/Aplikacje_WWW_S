using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGradeService
    {
        Task<IEnumerable<Grade>> GetGrades(CheckGradesVm checkGradesVm);
    }
}
