using SchoolRegister.ViewModels.VM;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.Services.Interfaces
{
  public interface IGradeService
  {
    Task<IEnumerable<Grade>> GetGradesAsync(GetGradesVm getGradesVm);
  }
}