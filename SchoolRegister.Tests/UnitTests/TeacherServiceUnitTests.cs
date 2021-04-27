using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;
using System.Data;
using System.Threading.Tasks;
using SchoolRegister.Model.DataModels;


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
        public async void AddGradeToStudentByTeacher()
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
        public async void SendEmailByTeacherToParent()
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
    }
}