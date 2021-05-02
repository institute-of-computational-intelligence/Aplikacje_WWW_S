using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolRegister.DAL.EF;
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
        public void AddNewSubjectWithNoIdValue()
        {
            var subject = new AddOrUpdateSubjectVm()
            {
                Name = "Discrete mathematics",
                Description = "The purpose of this course is to understand and use (abstract) discrete structures that are backbones of computer science.",
                TeacherId = 12
            };
            var subjectReport = _subjectService.AddOrUpdateSubject(subject);
            Assert.NotNull(subjectReport);
        }

        [Fact]
        public void AddNewSubjectWithIdZero()
        {
            var subject = new AddOrUpdateSubjectVm()
            {
                Id = 0,
                Name = "Discrete mathematics",
                Description = "The purpose of this course is to understand and use (abstract) discrete structures that are backbones of computer science.",
                TeacherId = 12
            };
            var subjectReport = _subjectService.AddOrUpdateSubject(subject);
            Assert.NotNull(subjectReport);
        }

        [Fact]
        public void UpdateExistingSubjectWithCorrectId()
        {
            var subject = new AddOrUpdateSubjectVm()
            {
                Id = 5,
                Name = "Discrete mathematics",
                Description = "The purpose of this course is to understand and use (abstract) discrete structures that are backbones of computer science.",
                TeacherId = 2
            };
            var subjectReport = _subjectService.AddOrUpdateSubject(subject);
            Assert.NotNull(subjectReport);
            Assert.Matches(subject.Name, DbContext.Subjects.FirstOrDefault(s => s.Id == subject.Id).Name);
        }

        [Fact]
        public void GetSubject()
        {
            var subjectReport = _subjectService.GetSubject(sb => sb.Name == "Advanced Internet Programming");
            Assert.NotNull(subjectReport);
        }

        [Fact]
        public void GetSubjects()
        {
            var subjectReport = _subjectService.GetSubjects(sb => sb.TeacherId == 1);
            Assert.NotNull(subjectReport);
            Assert.Equal(2, subjectReport.Count());
        }

        [Fact]
        public void GetTheAllSubjects()
        {
            var subjectReport = _subjectService.GetSubjects(null);
            Assert.NotNull(subjectReport);
            Assert.Equal(DbContext.Subjects.Count(), subjectReport.Count());
        }
    }
}