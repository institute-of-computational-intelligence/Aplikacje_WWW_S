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
        public async void DisplayGrades_ByStudent()
        {
            var getGradesReportForStudent = new GetGradesVm()
            {
                StudentId = 5,
                CallerId = 5
            };

            var gradesReport = await _gradeService.DisplayGrades(getGradesReportForStudent);
            Assert.NotNull(gradesReport);
        }

        [Fact]
        public async void DisplayGrades_ByParent()
        {
            var getGradesReportForStudent = new GetGradesVm()
            {
                StudentId = 7,
                CallerId = 3
            };

            var gradesReport = await _gradeService.DisplayGrades(getGradesReportForStudent);
            Assert.NotNull(gradesReport);
        }

        [Fact]
        public async void DisplayGrades_CallerInvalidRole()
        {
            var getGradesReportForStudent = new GetGradesVm()
            {
                StudentId = 7,
                CallerId = 12
            };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            {
                return _gradeService.DisplayGrades(getGradesReportForStudent);
            });
        }

        [Fact]
        public async void DisplayGrades_NullCheck()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
            {
                return _gradeService.DisplayGrades(null);
            });
        }

        [Fact]
        public async void DisplayGrades_InvalidStudent()
        {
            var getGradesReportForStudent = new GetGradesVm()
            {
                StudentId = 22,
                CallerId = 12
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
            {
                return _gradeService.DisplayGrades(getGradesReportForStudent);
            });
        }

        [Fact]
        public async void DisplayGrades_InvalidParent()
        {
            var getGradesReportForStudent = new GetGradesVm()
            {
                StudentId = 5,
                CallerId = 22
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
            {
                return _gradeService.DisplayGrades(getGradesReportForStudent);
            });
        }

        [Fact]
        public async void DisplayGrades_NotStudentParent()
        {
            var getGradesReportForStudent = new GetGradesVm()
            {
                StudentId = 5,
                CallerId = 4
            };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            {
                return _gradeService.DisplayGrades(getGradesReportForStudent);
            });
        }
    }
}