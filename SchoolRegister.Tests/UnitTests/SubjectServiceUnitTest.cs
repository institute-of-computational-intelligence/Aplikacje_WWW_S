using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;

namespace SchoolRegister.Tests.UnitTests
{
    public class SubjectServiceUnitTest : BaseUnitTests
    {
        private readonly ISubjectService _subjectService;
        public SubjectServiceUnitTest(ApplicationDbContext dbContext, ISubjectService subjectService) : base(dbContext)
        {
            _subjectService = subjectService;
        }

        [Fact]
        public void AddOrUpdateSubject_AddSubject()
        {
            var subjectVm = new AddOrUpdateSubjectVm()
            {
                Name = "Systemy wbudowane",
                Description = "Podstawy mikroprocesorów",
                TeacherId = 12
            };

            var result = _subjectService.AddOrUpdateSubject(subjectVm);
            Assert.NotNull(result);
            Assert.Equal(6, DbContext.Subjects.Count());
        }

        [Fact]
        public void AddOrUpdateSubject_UpdateSubject()
        {
            var subjectVm = new AddOrUpdateSubjectVm()
            {
                Id = 2,
                Name = "Programowanie obiektowe i funkcyjne",
                Description = "Programowanie obiektowe jest przedmiotem realizującym przykłady programowanie obiektowego",
                TeacherId = 12
            };

            var result = _subjectService.AddOrUpdateSubject(subjectVm);
            Assert.NotNull(result);
            Assert.Equal(5, DbContext.Subjects.Count());
            Assert.Matches(subjectVm.Name, DbContext.Subjects.FirstOrDefault(x => x.Id == subjectVm.Id).Name);
        }

        [Fact]
        public void GetSubject_ValidIdTest()
        {
            var result = _subjectService.GetSubject(x => x.Id == 3);
            Assert.NotNull(result);
            Assert.Equal(3, result.Id);
        }

        [Fact]
        public void GetSubjects_GetThreeSubjectsTest()
        {
            var result = _subjectService.GetSubjects(x => x.Id <= 3);
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

    }
}