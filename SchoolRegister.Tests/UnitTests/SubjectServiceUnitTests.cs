using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;
using System.Linq.Expressions;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.Tests.UnitTests
{
  public class SubjectServiceUnitTests : BaseUnitTests
  {
    private readonly ISubjectService _subjectService;

    public SubjectServiceUnitTests(ApplicationDbContext dbContext, ISubjectService subjectService) : base(dbContext)
    {
      _subjectService = subjectService;
    }

    [Fact]
    public void GetSubject()
    {
      Expression<Func<Subject, bool>> filterSubject = subject => subject.Name == "Subject";

      var subject = _subjectService.GetSubject(filterSubject);
      Assert.NotNull(subject);
    }

    [Fact]
    public void GetSubjectError()
    {
      Expression<Func<Subject, bool>> filterSubject = subject => subject.Name == "Subject Subject";

      var subject = _subjectService.GetSubject(filterSubject);
      Assert.Null(subject);
    }

    [Fact]
    public void GetSubjects()
    {
      Expression<Func<Subject, bool>> filterSubject = subject => subject.TeacherId == 4;

      var subjects = _subjectService.GetSubjects(filterSubject);
      Assert.NotNull(subjects);
    }

    [Fact]
    public void AddSubject()
    {
      var addOrUpdateSubject = new AddOrUpdateSubjectVm()
      {
        Id = 3,
        Name = "Subject",
        Description = "...",
        TeacherId = 3
      };

      var newSubject = _subjectService.AddOrUpdateSubject(addOrUpdateSubject);

      Assert.NotNull(newSubject);
      Assert.Equal("Subject", newSubject.Name);
      Assert.Equal("...", newSubject.Description);
      Assert.Equal(3, newSubject.TeacherId);
    }
  }
}