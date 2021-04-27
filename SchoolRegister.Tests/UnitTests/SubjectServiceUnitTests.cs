using System;
using System.Collections.Generic;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.Services.Services;
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
        public void Given_NullParameter_When_CallingAddOrUpdateSubject_Then_ThrowsArgumentNullException()
        {

            Assert.Throws<ArgumentNullException>(() => _subjectService.AddOrUpdateSubject(null));         
        }


        [Fact]
        public void Given_NullParameter_When_CallingGetSubject_Then_ThrowsArgumentNullException()
        {

            Assert.Throws<ArgumentNullException>(() => _subjectService.GetSubject(null));         
        }

        // do poprawy po uzyksaniu info 
        [Fact]
        public void Given_ValidParameter_When_CallingAddOrUpdateSubject_Then_AddingOrUpdateSubject()
        {
            var getSubjectToAddOrUpdate = new AddOrUpdateSubjectVm()
            {
                Id = 1,
                Name = "Biology",
                Description = "Very imporatnt subject",
                TeacherId = 2,
            };
            
            var subjectToAddOrUpdate = _subjectService.AddOrUpdateSubject(getSubjectToAddOrUpdate);

            Assert.NotNull(subjectToAddOrUpdate);
        
        }

        [Fact]
        public void Given_ValidParameter_When_GetSubject_Then_GetingSubject()
        {        
            var subjectToGet = _subjectService.GetSubject(subject => subject.Name.Equals("Biology"));
            Assert.NotNull(subjectToGet);
        }

        [Fact]
        public void Given_ValidParameter_When_GetSubjects_Then_GetingSubjects()
        {
            var subjects = _subjectService.GetSubjects(x => x.Id > 2 && x.Id <= 4);
            Assert.NotNull(subjects);
            Assert.NotEmpty(subjects);
            Assert.Equal(2, subjects.Count());
        }
    }
}