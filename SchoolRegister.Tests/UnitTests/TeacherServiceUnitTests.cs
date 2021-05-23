using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System;
using System.Linq;
using System.Threading.Tasks;
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
        public async void AddGradeToStudentByTeacher()
        {
            var countBefore = DbContext.Grades.Count();

            var addGradeToStudent = new AddGradeToStudentVm()
            {
                StudentId = 5,
                TeacherId = 2,
                SubjectId = 3,
                Grade = GradeScale.DST
            };

            var grade = await _teacherService.AddGradeToStudentAsync(addGradeToStudent);
            var countAfter = DbContext.Grades.Count();

            Assert.NotNull(grade);
            Assert.Contains(grade, DbContext.Grades);
            Assert.Equal(GradeScale.DST, grade.GradeValue);
            Assert.Equal(5, grade.StudentId);
            Assert.True(countAfter > countBefore);
        }

        [Fact]
        public async void AddGradeToNotStudentByTeacher()
        {
            var countBefore = DbContext.Grades.Count();

            var addGradeToStudent = new AddGradeToStudentVm()
            {
                StudentId = 3,
                TeacherId = 2,
                SubjectId = 3,
                Grade = GradeScale.DST
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() => _teacherService.AddGradeToStudentAsync(addGradeToStudent));
            var countAfter = DbContext.Grades.Count();
            Assert.Equal(countBefore, countAfter);
        }

        [Fact]
        public async void AddGradeToStudentByParent()
        {
            var countBefore = DbContext.Grades.Count();

            var addGradeToStudent = new AddGradeToStudentVm()
            {
                StudentId = 5,
                TeacherId = 3,
                SubjectId = 3,
                Grade = GradeScale.DST
            };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _teacherService.AddGradeToStudentAsync(addGradeToStudent));
            var countAfter = DbContext.Grades.Count();
            Assert.Equal(countBefore, countAfter);
        }


        [Fact]
        public async void AddGradeToStudentByStudent()
        {
            var countBefore = DbContext.Grades.Count();

            var addGradeToStudent = new AddGradeToStudentVm()
            {
                StudentId = 5,
                TeacherId = 5,
                SubjectId = 3,
                Grade = GradeScale.DST
            };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _teacherService.AddGradeToStudentAsync(addGradeToStudent));
            var countAfter = DbContext.Grades.Count();
            Assert.Equal(countBefore, countAfter);
        }

        [Fact]
        public async void AddGradeToStudentByTeacherNonExistingSubject()
        {
            var countBefore = DbContext.Grades.Count();

            var addGradeToStudent = new AddGradeToStudentVm()
            {
                StudentId = 5,
                TeacherId = 2,
                SubjectId = -1,
                Grade = GradeScale.DST
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() => _teacherService.AddGradeToStudentAsync(addGradeToStudent));
            var countAfter = DbContext.Grades.Count();
            Assert.Equal(countBefore, countAfter);
        }

        [Fact]
        public async void SendEmailByTeacherToParent()
        {
            var sendEmail = new SendEmailVm()
            {
                TeacherId = 2,
                ParentId = 3,
                EmailSubject = "??",
                EmailContent = ""
            };

            var exception = await Record.ExceptionAsync(() => Task.Run(() => _teacherService.SendEmailAsync(sendEmail)));

            Assert.Null(exception);
        }

        [Fact]
        public void SendEmailByParentToStudent()
        {
            var sendEmail = new SendEmailVm()
            {
                TeacherId = 12,
                ParentId = 6,
                EmailSubject = "??",
                EmailContent = ""
            };

            Assert.ThrowsAsync<UnauthorizedAccessException>(() => Task.Run(() => _teacherService.SendEmailAsync(sendEmail)));
        }

        [Fact]
        public void SendEmailByNonExistingUser()
        {
            var sendEmail = new SendEmailVm()
            {
                TeacherId = -1,
                ParentId = 3,
                EmailSubject = "??",
                EmailContent = ""
            };

            Assert.ThrowsAsync<ArgumentNullException>(() => Task.Run(() => _teacherService.SendEmailAsync(sendEmail)));
        }
    }
}