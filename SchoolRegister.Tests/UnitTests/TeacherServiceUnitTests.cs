using System;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;
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
        public async void AddGradeTeacher(){
            var addGradeToStudent = new AddGradeVm()
            {
                StudentId = 1,
                TeacherId = 2,
                SubjectId = 3,
                Grade = GradeScale.BDB
            };

            var grade = await _teacherService.AddGradeAsync(addGradeToStudent);

            Assert.NotNull(grade);
            Assert.Contains(grade, DbContext.Grades);
            Assert.Equal(GradeScale.BDB, grade.GradeValue);
        }
        [Fact]
        public async void AddGradeStudent()
        {
            var addGradeToStudent = new AddGradeVm()
            {
                StudentId = 2,
                TeacherId = 1,
                SubjectId = 1,
                Grade = GradeScale.BDB
            };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _teacherService.AddGradeAsync(addGradeToStudent));
        }

        [Fact]
        public async void AddGradeParent(){
            var addGradeToStudent = new AddGradeVm()
            {
                StudentId = 2,
                TeacherId = 1,
                SubjectId = 1,
                Grade = GradeScale.DST
            };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _teacherService.AddGradeAsync(addGradeToStudent));
        }

        [Fact]
        public async void AddGradeNullSubject()
        {
            var addGradeToStudent = new AddGradeVm()
            {
                StudentId = 2,
                TeacherId = 1,
                SubjectId = 9090,
                Grade = GradeScale.DST
            };
            await Assert.ThrowsAsync<ArgumentNullException>(() => _teacherService.AddGradeAsync(addGradeToStudent));
        }

    }
}