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
        public async void GetGradesReportForStudentByStudent()
        {
            var getGradesReportForStudent = new GradesVm()
            {
                StudentId = 7,
                CallerId = 7
            };
            var gradesReport = await _gradeService.ShowGrades(getGradesReportForStudent);
            Assert.NotNull(gradesReport);
        }
        [Fact]
        public async void GetGradesReportForStudentByParent()
        {
            var getGradesReportForStudent = new GradesVm()
            {
                StudentId = 5,
                CallerId = 3
            };
            var gradesReport = await _gradeService.ShowGrades(getGradesReportForStudent);
            Assert.NotNull(gradesReport);
        }

        [Fact]
        public async void GetGradesReportBySubjectForStudentByStudent()
        {
            var getGradesReportForStudent = new GradesVm()
            {
                StudentId = 5,
                CallerId = 5,
                SubjectId = 3
            };
            var gradesReport = await _gradeService.ShowGrades(getGradesReportForStudent);
            Assert.NotNull(gradesReport);
        }
        [Fact]
        public async void GetGradesReportBySubjectForStudentByParent()
        {
            var getGradesReportForStudent = new GradesVm()
            {
                StudentId = 10,
                CallerId = 4,
                SubjectId = 3
            };
            var gradesReport = await _gradeService.ShowGrades(getGradesReportForStudent);
            Assert.NotNull(gradesReport);
        }

        [Fact]
        public async void GetGradesReport_InvalidCallerRole()
        {
            var getGradesReportForStudent = new GradesVm()
            {
                StudentId = 7,
                CallerId = 2
            };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            {
                return _gradeService.ShowGrades(getGradesReportForStudent);
            });
        }

        [Fact]
        public async void GetGradesReportByStudent_WrongId()
        {
            var getGradesReportForStudent = new GradesVm()
            {
                StudentId = 8,
                CallerId = 7
            };

            await Assert.ThrowsAsync<ArgumentException>(() =>
            {
                return _gradeService.ShowGrades(getGradesReportForStudent);
            });
        }

        [Fact]
        public async void GetGradesReport_NoValues()
        {
            var getGradesReportForStudent = new GradesVm();
           
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
            {
                return _gradeService.ShowGrades(getGradesReportForStudent);
            });
        }
    }
}
