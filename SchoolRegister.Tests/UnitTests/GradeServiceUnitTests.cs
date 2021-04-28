using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;
using SchoolRegister.Tests;

namespace SchoolRegister.Tests.UnitTests 
{
    public class GradeServiceUnitTests: BaseUnitTests 
    {
        private readonly IGradeService _gradeService;
        public GradeServiceUnitTests(ApplicationDbContext dbContext, IGradeService gradeService): base(dbContext) 
        {
                _gradeService = gradeService;
        }
            [Fact]
        public void GetGradesForStudentByTeacher() 
        {
            var getGradesForStudent = new CheckGradesVm() {
                CurrentUserId = 5,
                    StudentId = 1
            };
            var gradesReport = _gradeService.GetGrades(getGradesForStudent);
            Assert.NotNull(gradesReport);
        }
    }
}