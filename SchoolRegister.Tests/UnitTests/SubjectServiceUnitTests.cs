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

        [Fact]
        public void Given_NullParameter_When_CallingGetSubjects_Then_ThrowsArgumentNullException()
        {

            Assert.Throws<ArgumentNullException>(() => _subjectService.GetSubjects(null));         
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
            var getSubject = new SubjectVm()
            {
                Id  = 2,
                Name = "Biology",       
                Description = "Very imporatnt subject",    
                TeacherName = "Jan Kowalski",
                TeacherId  = 5
            };
            
            var subjectToGet = _subjectService.GetSubject(subject => subject.Equals(getSubject.Name));
            Assert.NotNull(subjectToGet);
        }

        /*[Fact]
        public void Given_ValidParameter_When_GetSubjects_Then_GetingSubjects()
        {
            var getSubject = new List<SubjectVm>()
            {
                new SubjectVm(){ Name = "History"},
                new SubjectVm(){ Name = "Biology"},
                new SubjectVm(){ Name = "Math"}
            };

            var subjectsToGet = _subjectService.GetSubjects(subject )
        }*/
    }
}