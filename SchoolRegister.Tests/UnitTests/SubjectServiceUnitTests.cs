using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
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
                Name= "Advanced Data Base systems",
                Description = "Subject about creating and managing data bases in SQL",
                TeacherId = 12
            };
            var subjectReport =  _subjectService.AddOrUpdateSubject(subject);
            Assert.NotNull(subjectReport);
        }

        [Fact]
        public void AddNewSubjectWithIdZero()
        {
            var subject = new AddOrUpdateSubjectVm()
            {
                Id = 0,
                Name= "Advanced Data Base systems",
                Description = "Subject about creating and managing data bases in SQL",
                TeacherId = 12
            };
            var subjectReport =  _subjectService.AddOrUpdateSubject(subject);
            Assert.NotNull(subjectReport);
        }

        [Fact]
        public void UpdateExistingSubjectWithCorrectId()
        {
            var subject = new AddOrUpdateSubjectVm()
            {
                Id = 5,
                Name= "Advanced Data Base systems",
                Description = "Subject about creating and managing data bases in SQL",
                TeacherId = 2
            };
            var subjectReport =  _subjectService.AddOrUpdateSubject(subject);
            Assert.NotNull(subjectReport);
            Assert.Matches(subject.Name, DbContext.Subjects.FirstOrDefault(s => s.Id == subject.Id).Name);
        }

        [Fact]
        public void GetSubject()
        {
            var subjectReport =  _subjectService.GetSubject(sb => sb.Name == "Advanced Internet Programming");
            Assert.NotNull(subjectReport);
        }

        [Fact]
        public void GetSubjects()
        {
            var subjectReport =  _subjectService.GetSubjects(sb => sb.TeacherId == 1);
            Assert.NotNull(subjectReport);
            Assert.Equal(2,subjectReport.Count());
        }

        [Fact]
        public void GetTheAllSubjects()
        {
            var subjectReport =  _subjectService.GetSubjects(null);
            Assert.NotNull(subjectReport);
            Assert.Equal(DbContext.Subjects.Count(),subjectReport.Count());
        }
    }
}
