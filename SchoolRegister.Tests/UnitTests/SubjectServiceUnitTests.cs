using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;
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
        public void AddNewSubjectWithNoId()
        {
            var subject = new AddOrUpdateSubjectVm()
            {
                Name = "Strony internetowe",
                Description = "Tworzenie stron...",
                TeacherId = 1
            };
            var subjectReport = _subjectService.AddOrUpdateSubject(subject);
            Assert.NotNull(subjectReport);
        }

        [Fact]
        public void AddNewSubjec()
        {
            var subject = new AddOrUpdateSubjectVm()
            {
                Id = 1,
                Name = "Aplikacje www",
                Description = "Tworzenie aplikacji www...",
                TeacherId = 2
            };
            var subjectReport = _subjectService.AddOrUpdateSubject(subject);
            Assert.NotNull(subjectReport);
        }

        [Fact]
        public void UpdateSubject()
        {
            var subject = new AddOrUpdateSubjectVm()
            {
                Id = 1,
                Name = "Aplikacje www1",
                Description = "tworzenie aplikacji wwww",
                TeacherId = 2
            };
            var subjectReport = _subjectService.AddOrUpdateSubject(subject);
            Assert.NotNull(subjectReport);
            Assert.Matches(subject.Name, DbContext.Subjects.FirstOrDefault(s => s.Id == subject.Id).Name);
        }

        [Fact]
        public void GetSubject()
        {
            var subjectReport = _subjectService.GetSubject(sb => sb.Name == "Aplikacje www");
            Assert.NotNull(subjectReport);
        }

        [Fact]
        public void GetSubject1()
        {
            var subjectReport = _subjectService.GetSubjects(sb => sb.TeacherId == 1);
            Assert.NotNull(subjectReport);
            Assert.Equal(2, subjectReport.Count());
        }

        [Fact]
        public void GetAllSubjects()
        {
            var subjectReport = _subjectService.GetSubjects(null);
            Assert.NotNull(subjectReport);
            Assert.Equal(DbContext.Subjects.Count(), subjectReport.Count());
        }
    }
}