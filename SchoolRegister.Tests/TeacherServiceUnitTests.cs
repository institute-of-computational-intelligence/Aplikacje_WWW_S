using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;
using System.Data;
using System.Threading.Tasks;

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
        public async void AddGradeByTeacher()
        {
            var countBefore = DbContext.Grades.Count();
            var addGradeToStudent = new AddGradeAsyncVm()
            {
                StudentId = 5,
                TeacherId = 2,
                SubjectId = 3,
                GradeValue = GradeScale.DST
            };

            var grade = await _teacherService.AddGradeAsync(addGradeToStudent);
            var countAfter = DbContext.Grades.Count();

            Assert.NotNull(grade);
            Assert.Contains(grade, DbContext.Grades);
            Assert.Equal(GradeScale.DST, grade.GradeValue);
            Assert.Equal(5, grade.StudentId);
            Assert.True(countAfter > countBefore);
        }

        [Fact]
        public async void AddGradeByTeacher_ToUser()
        {
            var countBefore = DbContext.Grades.Count();
            var addGradeToStudent = new AddGradeAsyncVm()
            {
                StudentId = 3,
                TeacherId = 2,
                SubjectId = 3,
                GradeValue = GradeScale.DST
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() => _teacherService.AddGradeAsync(addGradeToStudent));
            var countAfter = DbContext.Grades.Count();
            Assert.Equal(countBefore, countAfter);
        }

        [Fact]
        public async void AddGradeByParent()
        {
            var countBefore = DbContext.Grades.Count();
            var addGradeToStudent = new AddGradeAsyncVm()
            {
                StudentId = 5,
                TeacherId = 3,
                SubjectId = 3,
                GradeValue = GradeScale.DST
            };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _teacherService.AddGradeAsync(addGradeToStudent));
            var countAfter = DbContext.Grades.Count();
            Assert.Equal(countBefore, countAfter);
        }


        [Fact]
        public async void AddGradeByStudent()
        {
            var countBefore = DbContext.Grades.Count();
            var addGradeToStudent = new AddGradeAsyncVm()
            {
                StudentId = 5,
                TeacherId = 5,
                SubjectId = 3,
                GradeValue = GradeScale.DST
            };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _teacherService.AddGradeAsync(addGradeToStudent));
            var countAfter = DbContext.Grades.Count();
            Assert.Equal(countBefore, countAfter);
        }

        [Fact]
        public async void AddGradeByTeacher_NoSubject()
        {
            var countBefore = DbContext.Grades.Count();
            var addGradeToStudent = new AddGradeAsyncVm()
            {
                StudentId = 5,
                TeacherId = 2,
                SubjectId = -1,
                GradeValue = GradeScale.DST
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() => _teacherService.AddGradeAsync(addGradeToStudent));
            var countAfter = DbContext.Grades.Count();
            Assert.Equal(countBefore, countAfter);
        }

        [Fact]
        public async void SendEmailByTeacher_ToParent()
        {
            var sendEmail = new SendEmailVm()
            {
                SenderId = 2,
                RecipientId = 3,
                EmailSubject = "??",
                EmailBody = ""
            };

            var exception = await Record.ExceptionAsync(() => Task.Run(() => _teacherService.SendEmailToParent(sendEmail)));

            Assert.Null(exception);
        }

        [Fact]
        public void SendEmailByParent()
        {
            var sendEmail = new SendEmailVm()
            {
                SenderId = 12,
                RecipientId = 6,
                EmailSubject = "??",
                EmailBody = ""
            };

            Assert.ThrowsAsync<UnauthorizedAccessException>(() => Task.Run(() => _teacherService.SendEmailToParent(sendEmail)));
        }
    }
}