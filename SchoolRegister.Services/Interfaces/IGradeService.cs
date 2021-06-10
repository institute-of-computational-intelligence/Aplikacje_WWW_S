using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGradeService
    {
        Task<IEnumerable<Grade>> GetGrades(GradeVm gradeVm);
        GradesReportVm GetGradesReportForStudent(GetGradesReportVm getGradesVm);
        GradeVm AddGradeToStudent(AddGradeVm addGradeToStudentVm);
    }
}