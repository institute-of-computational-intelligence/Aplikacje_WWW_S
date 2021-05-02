using System;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.Tests.UnitTests
{
  public class TeacherServiceUnitTests : BaseUnitTests
  {
    private readonly ITeacherService _teacherService;

    public TeacherServiceUnitTests(ApplicationDbContext dbContext, ITeacherService teacherService) : base(dbContext)
    {
      _teacherService = teacherService;
    }
    [Fact]
    public async void AddGrade()
    {
      var addGrade = new AddGradeAsyncVm()
      {
        StudentId = 8,
        SubjectId = 4,
        TeacherId = 1,
        GradeValue = GradeScale.DB
      };

      var grade = await _teacherService.AddGradeAsync(addGrade);

      Assert.NotNull(grade);
      Assert.Contains(grade, DbContext.Grades);
      Assert.Equal(8, grade.StudentId);
      Assert.Equal(4, grade.SubjectId);
      Assert.Equal(GradeScale.DB, grade.GradeValue);
    }
    [Fact]
    public async void AddGradeError()
    {
      var addGradeToStudent = new AddGradeAsyncVm()
      {
        StudentId = 4,
        SubjectId = 33,
        GradeValue = GradeScale.DB
      };
      await Assert.ThrowsAsync<ArgumentNullException>(() => _teacherService.AddGradeAsync(addGradeToStudent));
    }

  }
}