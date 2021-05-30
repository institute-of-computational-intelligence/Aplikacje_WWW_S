using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;
using System.Linq.Expressions;

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
        public void UpdateSubject()
        {
            var countBefore = DbContext.Subjects.Count();
            var addOrUpdateSubject = new AddOrUpdateSubjectVm()
            {
                Id = 1,
                Name = "Programowanie niskopoziomowe",
                Description = "Programowanie niskopoziomowe w C++",
                TeacherId = 2
            };

            var updatedSubject = _subjectService.AddOrUpdateSubject(addOrUpdateSubject);
            var countAfter = DbContext.Subjects.Count();

            Assert.NotNull(updatedSubject);
            Assert.Equal("Programowanie niskopoziomowe w C++", updatedSubject.Description); 
            Assert.Equal(3, updatedSubject.TeacherId);
            Assert.Equal(countBefore, countAfter);
        }

        [Fact]
        public void AddSubject()
        {
            var beforeUpd = DbContext.Subjects.Count();
            var addOrUpdateSubject = new AddOrUpdateSubjectVm()
            {
                Name = "Projektowanie Systemów Informatycznych",
                Description = "Projektowanie i tworzenie systemów",
                TeacherId = 1
            };

            var Subject = _subjectService.AddOrUpdateSubject(addOrUpdateSubject);
            var afterUpd = DbContext.Subjects.Count();


            Assert.NotNull(Subject);
            Assert.True(afterUpd - beforeUpd == 1);
        }

        [Fact]
        public void GetSubjectFound()
        {
            Expression<Func<Subject, bool>> filterSubject = subject => subject.Name == "Programowanie stron internetowych";

            var getSubject = _subjectService.GetSubject(filterSubject);

            Assert.NotNull(getSubject);
        }

        [Fact]
        public void GetSubjectNotFound()
        {
            Expression<Func<Subject, bool>> filterSubject = subject => subject.Name == "Angielski biznesu";

            var getSubject = _subjectService.GetSubject(filterSubject);

            Assert.Null(getSubject);
        }

        [Fact]
        public void GetSubjectsFound()
        {
            Expression<Func<Subject, bool>> filterSubject = subject => subject.TeacherId == 2;

            var getSubjects = _subjectService.GetSubjects(filterSubject);

            Assert.NotNull(getSubjects);
            Assert.Equal(2, getSubjects.Count());
        }

        [Fact]
        public void GetSubjectsZero()
        {
            Expression<Func<Subject, bool>> filterSubject = subject => subject.Id == -1;

            var getSubjects = _subjectService.GetSubjects(filterSubject);

            Assert.NotNull(getSubjects);
            Assert.Empty(getSubjects);
        }

        [Fact]
        public void GetSubjectsCollectionCheck()
        {
            Expression<Func<Subject, bool>> filterSubject = subject => subject.Name.StartsWith("Programowanie");

            var getSubjects = _subjectService.GetSubjects(filterSubject);

            Assert.NotNull(getSubjects);
            Assert.Equal(new[] { "Programowanie obiektowe", "Programowanie Stron WWW" }, getSubjects.Select(s => s.Name).ToArray());
        }
    }
}