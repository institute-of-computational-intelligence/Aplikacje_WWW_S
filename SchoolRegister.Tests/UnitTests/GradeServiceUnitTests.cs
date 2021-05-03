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
      var getGradesReportForStudent = new GetGradesVm()
      {
        StudentId = 5,
        UserId = 1
      };
      var gradesReport = _gradeService.GetGradesAsync(getGradesReportForStudent);
      Assert.NotNull(gradesReport);
    }
    [Fact]
    public void GetGradesReportForStudentByStudent()
    {
      var getGradesReportForStudent = new GetGradesVm()
      {
        StudentId = 5,
        UserId = 5
      };
      var gradesReport = _gradeService.GetGradesAsync(getGradesReportForStudent);
      Assert.NotNull(gradesReport);
    }
    [Fact]
    public void GetGradesReportForStudentByParent()
    {
      var getGradesReportForStudent = new GetGradesVm()
      {
        StudentId = 5,
        UserId = 3
      };
      var gradesReport = _gradeService.GetGradesAsync(getGradesReportForStudent);
      Assert.NotNull(gradesReport);
    }
  }
}