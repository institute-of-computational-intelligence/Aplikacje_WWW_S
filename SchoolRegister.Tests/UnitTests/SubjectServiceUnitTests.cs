using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System;
using System.Linq;
using System.Linq.Expressions;
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
        public void UpdateSubject()
        {
            var countBefore = DbContext.Subjects.Count();

            var addOrUpdateSubject = new AddOrUpdateSubjectVm()
            {
                Id = 1,
                Name = "Aplikacje WWW",
                Description = "Aplikacje webowe .NET",
                TeacherId = 2
            };

            var updatedSubject = _subjectService.AddOrUpdateSubject(addOrUpdateSubject);
            var countAfter = DbContext.Subjects.Count();

            Assert.NotNull(updatedSubject);
            Assert.Equal("Aplikacje webowe .NET", updatedSubject.Description); // Description should be updated to "Aplikacje webowe .NET" from "Aplikacje webowe" 
            Assert.Equal(2, updatedSubject.TeacherId); // and new teacher should be a teacher with id = 2
            Assert.Equal(countBefore, countAfter); // Number of subjects should not change
        }

        [Fact]
        public void AddSubject()
        {
            var countBefore = DbContext.Subjects.Count();

            var addOrUpdateSubject = new AddOrUpdateSubjectVm()
            {
                Name = "Modelowanie i symulacja",
                Description = "Matlab i Blender",
                TeacherId = 1
            };

            var addSubject = _subjectService.AddOrUpdateSubject(addOrUpdateSubject);
            var countAfter = DbContext.Subjects.Count();


            Assert.NotNull(addSubject);
            Assert.True(countAfter - countBefore == 1); // Number of subjects should increase by exactly one subject
        }

        [Fact]
        public void GetSubjectFound()
        {
            Expression<Func<Subject, bool>> filterSubject = subject => subject.Name == "Advanced Internet Programming";

            var getSubject = _subjectService.GetSubject(filterSubject);

            Assert.NotNull(getSubject);
        }

        [Fact]
        public void GetSubjectNotFound()
        {
            Expression<Func<Subject, bool>> filterSubject = subject => subject.Name == "Zaawansowane programowanie obiektowe";

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
            Assert.Empty(getSubjects); // Collection should be empty
        }

        [Fact]
        public void GetSubjectsCollectionCheck()
        {
            Expression<Func<Subject, bool>> filterSubject = subject => subject.Name.StartsWith("Programowanie");

            var getSubjects = _subjectService.GetSubjects(filterSubject);

            Assert.NotNull(getSubjects);
            Assert.Equal(new[] { "Programowanie obiektowe", "Programowanie interaktywnej grafiki dla stron WWW" }, getSubjects.Select(s => s.Name).ToArray());
        }
    }
}