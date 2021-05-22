using System;
using System.Linq;
using AutoMapper;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.Services.Services;
using SchoolRegister.ViewModels.VM;
using Xunit;

namespace SchoolRegister.Tests.UnitTests {
    public class SubjectServiceUnitTests: BaseUnitTests {

        private readonly ISubjectService _subjectService;
        public SubjectServiceUnitTests (ISubjectService subjectService, ApplicationDbContext dbContext)
            :base(dbContext) {
            _subjectService = subjectService;           
        }

        [Fact]
        public void GetSubject () {
            var subject = _subjectService.GetSubject (x => x.Name == "Programowanie obiektowe");
            Assert.NotNull (subject);
        }

        [Fact]
        public void GetSubjects () {
            var subjects = _subjectService.GetSubjects (x => x.Id > 2 && x.Id <= 4);
            Assert.NotNull (subjects);
            Assert.NotEmpty(subjects);
            Assert.Equal (2, subjects.Count ());
        }

        [Fact]
        public void GetAllSubjects () {
            var subjects = _subjectService.GetSubjects ();
            Assert.NotNull (subjects);
            Assert.NotEmpty(subjects);
            Assert.Equal (DbContext.Subjects.Count (), subjects.Count ());
        }

        [Fact]
        public void AddNewSubject () {
            var newSubjectVm = new AddOrUpdateSubjectVm () {
                Name = "Zaawansowane programowanie internetowe",
                Description = "W ramach przedmiotu studenci tworzą rozwiazania w bibliotekach SPA",
                TeacherId = 1
            };
            var createdSubject = _subjectService.AddOrUpdateSubject (newSubjectVm);
            Assert.NotNull (createdSubject);
            Assert.Equal ("Zaawansowane programowanie internetowe", createdSubject.Name);
        }

        [Fact]
        public void EditSubject () {
            var editSubjectVm = new AddOrUpdateSubjectVm () {
                Id = 1,
                Name = "Aplikacje webowe",
                Description = null,
                TeacherId = 1
            };
            var editedSubjectVm = _subjectService.AddOrUpdateSubject (editSubjectVm);
            Assert.NotNull (editedSubjectVm);
            Assert.Equal ("Aplikacje webowe", editedSubjectVm.Name);
            Assert.Null (editedSubjectVm.Description);

        }

    }
}