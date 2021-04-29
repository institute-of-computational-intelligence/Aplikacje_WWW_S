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

        public SubjectServiceUnitTests(ApplicationDbContext dbContext, ISubjectService subjectService) : base(dbContext){
            _subjectService = subjectService;
        }

        [Fact]
        public void AddOrUpdateSubject(){
            var addOrUpdateSubject = new AddOrUpdateSubjectVm()
            {
                Id = 1,
                Name = "Przedmiot",
                Description = "Opis przedmiotu",
                TeacherId = 5
            };

            var newSubject = _subjectService.AddOrUpdateSubject(addOrUpdateSubject);

            Assert.NotNull(newSubject);
            Assert.Equal("Przedmiot", newSubject.Name);  
            Assert.Equal("Opis przedmiotu", newSubject.Description);  
            Assert.Equal(5, newSubject.TeacherId); 
        }

        [Fact]
        public void AddSubject(){
            var addOrUpdateSubject = new AddOrUpdateSubjectVm()
            {
                Name = "Przedmiot2",
                Description = "Opis2",
                TeacherId = 1
            };

            var newSubject = _subjectService.AddOrUpdateSubject(addOrUpdateSubject);
            Assert.NotNull(newSubject);
            Assert.Equal("Przedmiot2", newSubject.Name);  
            Assert.Equal("Opis2", newSubject.Description);  
            Assert.Equal(5, newSubject.TeacherId); 
        }

        [Fact]
        public void GetSubject(){
            Expression<Func<Subject, bool>> filterSubject = subject => subject.Name == "Przedmiot";

            var getSubject = _subjectService.GetSubject(filterSubject);
            Assert.NotNull(getSubject);
        }

        [Fact]
        public void GetSubjectNull(){
            Expression<Func<Subject, bool>> filterSubject = subject => subject.Name == "Przedmiot3";

            var getSubject = _subjectService.GetSubject(filterSubject);
            Assert.Null(getSubject);
        }

        [Fact]
        public void GetSubjects(){
            Expression<Func<Subject, bool>> filterSubject = subject => subject.TeacherId == 1;

            var getSubjects = _subjectService.GetSubjects(filterSubject);
            Assert.NotNull(getSubjects);
            Assert.Equal(5, getSubjects.Count());
        }

        [Fact]
        public void GetSubjectsEmpty() {
            Expression<Func<Subject, bool>> filterSubject = subject => subject.Id == -10;

            var getSubjects = _subjectService.GetSubjects(filterSubject);
            Assert.NotNull(getSubjects);
            Assert.Empty(getSubjects); 
        }
    }
}