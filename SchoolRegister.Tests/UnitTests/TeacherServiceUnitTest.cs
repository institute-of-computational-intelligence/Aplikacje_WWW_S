using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;

namespace SchoolRegister.Tests.UnitTests
{
    public class TeacherServiceUnitTest : BaseUnitTests
    {
        private readonly ITeacherService _teacherService;
        public TeacherServiceUnitTest(ApplicationDbContext dbContext, ITeacherService teacherService) : base(dbContext)
        {
            _teacherService = teacherService;
        }
        [Fact]
        public async void AddGradeToStudent_AddGrade()
        {
            var gradeVm = new AddGradeToStudentVm()
            {
                StudentId = 5,
                SubjectId = 1,
                GradeValue = GradeScale.DB,
                TeacherId = 1
            };

            var grade = await _teacherService.AddGradeToStudent(gradeVm);
            Assert.NotNull(grade);
            Assert.Equal(2, DbContext.Grades.Count());
            Assert.Equal(gradeVm.GradeValue, DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == gradeVm.StudentId).Grades.Last().GradeValue);
        }

        [Fact]
        public async void AddGradeToStudent_InvalidTeacherTest()
        {
            var gradeVm = new AddGradeToStudentVm()
            {
                StudentId = 5,
                SubjectId = 1,
                GradeValue = GradeScale.DB,
                TeacherId = 3
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() => _teacherService.AddGradeToStudent(gradeVm));
        }

        [Fact]
        public async void AddGradeToStudent_EmptyViewModelParametersTest()
        {
            var gradeVm = new AddGradeToStudentVm()
            {
                StudentId = 5,
                SubjectId = 1,
                GradeValue = GradeScale.DB,
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() => _teacherService.AddGradeToStudent(gradeVm));
        }



    }
}