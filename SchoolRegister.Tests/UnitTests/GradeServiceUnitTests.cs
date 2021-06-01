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
        public void GetGradesForStudentByTeacher()
        {
            var getGradesForStudent = new GetGradesVm()
            {
                StudentId = 5,
                UserId = 1
            };
            var gradesReport = _gradeService.GetGradesAsync(getGradesForStudent);
            Assert.NotNull(gradesReport);
        }
        [Fact]
        public void GetGradesForStudentByStudent()
        {
            var getGradesForStudent = new GetGradesVm()
            {
                StudentId = 5,
                UserId = 5
            };
            var gradesReport = _gradeService.GetGradesAsync(getGradesForStudent);
            Assert.NotNull(gradesReport);
        }
        [Fact]
        public void GetGradesForStudentByParent()
        {
            var getGradesForStudent = new GetGradesVm()
            {
                StudentId = 5,
                UserId = 3
            };
            var gradesReport = _gradeService.GetGradesAsync(getGradesForStudent);
            Assert.NotNull(gradesReport);
        }
    }
}