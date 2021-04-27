using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.Services.Services;
using SchoolRegister.ViewModels.VM;
using Xunit;

namespace SchoolRegister.Tests.UnitTests
{
    public class TeacherServiceUnitTests : BaseUnitTests
    {
        private readonly ITeacherService _teacherService;
        public TeacherServiceUnitTests(ApplicationDbContext dbContext, ITeacherService teacherService) : base(dbContext)
        {
            _teacherService = teacherService;
        }

        [Fact]
        public void Given_NullParameter_When_CallingAddGradeAsync_Then_ThrowsArgumentNullException()
        {

            Assert.Throws<ArgumentNullException>(() => _teacherService.AddGradeAsync(null));
        }

        [Fact]
        public void Given_NullParameter_When_CallingSendEmailToParent_Then_ThrowsArgumentNullException()
        {
            SendEmailVm sendEmailVm = null;

            Func<Task<bool>> act = () => _teacherService.SendEmailToParent(sendEmailVm);
            
            Assert.ThrowsAsync<ArgumentNullException>(act);
        }

        [Fact]
        public void Given_ValidParameter_When_CallingAddGradeAsync_Then_AddGrade()
        {
            var countBefore = DbContext.Grades.Count();
            var addGradeAsyncVm = new AddGradeAsyncVm()
            {
                StudentId = 2,
                TeacherId = 1,
                SubjectId = 5,
                GradeValue = (GradeScale)4
            };

            _teacherService.AddGradeAsync(addGradeAsyncVm);
            var countAfter = DbContext.Grades.Count();
            Assert.Equal(countAfter, countBefore);
        }

        [Fact]
        public void Given_ValidParameter_When_CallingSendEmailToParent_Then_SendingEmailToParent()
        {
            var sendEmailVm = new SendEmailVm()
            {
                SenderId = 3,
                RecipientId = 5,
                EmailSubject = "Child grades",
                EmailBody = "Your child get grate grades"
            };

            var act =  _teacherService.SendEmailToParent(sendEmailVm);
            Assert.NotNull(act);
        }
    }
}