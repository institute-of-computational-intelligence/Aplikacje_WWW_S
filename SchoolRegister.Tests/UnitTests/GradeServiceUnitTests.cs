using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;
namespace SchoolRegister.Tests.UnitTests
{
    public class GradeServiceUnitTests : BaseUnitTests
    {
        private readonly IGradeService _gradeService;
        public GradeServiceUnitTests(ApplicationDbContext dbContext, IGradeService gradeService) : base(dbContext)
        {
            _gradeService = gradeService;
        }
        
        
        [Fact]
        public void GetGradesReportForStudentByTeacher()
        {
            var getGradesReportForStudent = new GradesVm()
            {
                StudentId = 5,
                CallerId = 1
            };
            var gradesReport = _gradeService.ShowGrades(getGradesReportForStudent);
            Assert.NotNull(gradesReport);
        }
        [Fact]
        
        public void GetGradesReportForStudentByStudent()
        {
            var getGradesReportForStudent = new GradesVm()
            {
                StudentId = 5,
                CallerId = 5
            };
            var gradesReport = _gradeService.ShowGrades(getGradesReportForStudent);
            Assert.NotNull(gradesReport);
        }
        [Fact]
        public void GetGradesReportForStudentByParent()
        {
            var getGradesReportForStudent = new GradesVm()
            {
                StudentId = 5,
                CallerId = 3
            };
            var gradesReport = _gradeService.ShowGrades(getGradesReportForStudent);
            Assert.NotNull(gradesReport);
        }
        
        
    }
}
