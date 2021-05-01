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
    public class SubjectServiceUnitTests : BaseUnitTests
    {
        private readonly ISubjectService _subjectService;
        public SubjectServiceUnitTests(ApplicationDbContext dbContext, ISubjectService subjectService) : base(dbContext)
        {
            _subjectService = subjectService;
        }

        [Fact]
        public void UpdateSubjectTest()
        {
            var updateSubject = new AddOrUpdateSubjectVm()
            {
                Id = 1,
                Name = "Aplikacje WWW1",
                Description = "Aplikacje webowe",
                TeacherId = 2
            };
            var updatedSubject = _subjectService.AddOrUpdateSubject(updateSubject);
            Assert.NotNull(updatedSubject);
            Assert.Equal("Aplikacje WWW1",updatedSubject.Name);
        }
        [Fact]
        public void AddSubjectTest()
        {
            var updateSubject = new AddOrUpdateSubjectVm()
            {
                
                Name = "Aplikacje WWW1",
                Description = "Aplikacje webowe",
                TeacherId = 2
            };
            var updatedSubject = _subjectService.AddOrUpdateSubject(updateSubject);
            Assert.NotNull(updatedSubject);
        }

        [Fact]
        public void GetSubjectTest()
        {
            var subject = _subjectService.GetSubject (x => x.Id == 1);
            Assert.NotNull (subject);
        }
        [Fact]
        public void GetSubjectsTest () {
            var subjects = _subjectService.GetSubjects();

            Assert.NotNull (subjects);
            Assert.Equal (DbContext.Subjects.Count (), subjects.Count ());
        }

    }
}