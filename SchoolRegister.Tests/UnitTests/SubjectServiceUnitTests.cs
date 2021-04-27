using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;
using SchoolRegister.Tests;
using System.Linq.Expressions;

namespace SchoolRegister.Tests.UnitTests 
{
    public class SubjectServiceUnitTests: BaseUnitTests 
    {
        private readonly ISubjectService _subjectService;
        public SubjectServiceUnitTests(ApplicationDbContext dbContext, ISubjectService subjectService): base(dbContext) 
        {
             _subjectService = subjectService;
        }
        [Fact]
        public void UpdateSubject() 
        {
            var addOrUpdateSubject = new AddOrUpdateSubjectVm() {
                Id = 1,
                    Name = "Aplikacje WWW",
                    Description = "Aplikacje webowe .NET",
                    TeacherId = 2
            };

            var updatedSubject = _subjectService.AddOrUpdateSubject(addOrUpdateSubject);
            var countAfter = DbContext.Subjects.Count();

            Assert.NotNull(updatedSubject);
            Assert.Equal("Aplikacje webowe .NET", updatedSubject.Description);
            Assert.Equal(2, updatedSubject.TeacherId);

        }
        [Fact]
        public void AddSubject() 
        {
            var addSubject = new AddOrUpdateSubjectVm() {
                Id = 1,
                    Name = "Matematyka",
                    Description = "Królowa nauk",
                    TeacherId = 3
            };
            var subjectReport = _subjectService.AddOrUpdateSubject(addSubject);
            Assert.NotNull(subjectReport);
        }
        [Fact]
        public void GetSubjectForTeacher() 
        {
            Expression < Func < Subject, bool >> filterSubject = subject => subject.Name == "Advanced Internet Programming";
            var subjectReport = _subjectService.GetSubject(filterSubject);
            Assert.NotNull(subjectReport);
        }
        [Fact]
        public void GetSubjectFound() 
        {
            Expression < Func < Subject, bool >> filterSubject = subject => subject.Name == "Advanced Internet Programming";
            var getSubject = _subjectService.GetSubject(filterSubject);
            Assert.NotNull(getSubject);
        }

        [Fact]
        public void GetSubjectNotFound() 
        {
            Expression < Func < Subject, bool >> filterSubject = subject => subject.Name == "Zaawansowane programowanie obiektowe";
            var getSubject = _subjectService.GetSubject(filterSubject);
            Assert.Null(getSubject);
        }

        [Fact]
        public void GetSubjectsFound() 
        {
            Expression < Func < Subject, bool >> filterSubject = subject => subject.TeacherId == 2;
            var getSubjects = _subjectService.GetSubjects(filterSubject);
            Assert.NotNull(getSubjects);
            Assert.Equal(2, getSubjects.Count());
        }

        [Fact]
        public void GetSubjectsZero() {
            Expression < Func < Subject, bool >> filterSubject = subject => subject.Id == -1;
            var getSubjects = _subjectService.GetSubjects(filterSubject);
            Assert.NotNull(getSubjects);
            Assert.Empty(getSubjects); 
        }
        [Fact]
        public void GetSubjectsCollectionCheck() {
            Expression < Func < Subject, bool >> filterSubject = subject => subject.Name.StartsWith("Programowanie");
            var getSubjects = _subjectService.GetSubjects(filterSubject);
            Assert.NotNull(getSubjects);
            Assert.Equal(new [] {
                "Programowanie obiektowe",
                "Programowanie interaktywnej grafiki dla stron WWW"
            }, getSubjects.Select(s => s.Name).ToArray());
        }

    }
}