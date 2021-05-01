using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using SchoolRegister.Tests;
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
        public async void GetGradesReportForStudentByStudent()
        {
            var getGradesReportForStudent = new GradeVm()
            {
                StudentId = 5,
                UserId = 5
            };
            var gradesReport =await _gradeService.GetGrades(getGradesReportForStudent);
            Assert.NotNull(gradesReport);
        }
        [Fact]
        public async void GetGradesReportForStudentByParent()
        {
            var getGradesReportForStudent = new GradeVm()
            {
                StudentId = 5,
                UserId = 3
            };
            var gradesReport = await _gradeService.GetGrades(getGradesReportForStudent);
            Assert.NotNull(gradesReport);
        }
    }
}