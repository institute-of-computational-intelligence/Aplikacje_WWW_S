using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;
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
        public async void AddGradeToStudent1()
        {
            var newGrade = new AddGradeToStudentVm()
            {
                TeacherId= 2,
                Grade = GradeScale.DB,
                StudentId = 6,
                SubjectId = 3
            };
            var grade = await _teacherService.AddGradeToStudent(newGrade);
            Assert.NotNull(grade);
            Assert.Equal(2,DbContext.Grades.Count());
        }

        [Fact]
        public async void AddGradeToStudent_InvalidRole()
        {
            var newGrade = new AddGradeToStudentVm()
            {
                TeacherId= 7,
                Grade = GradeScale.DB,
                StudentId = 6,
                SubjectId = 1
            };
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            {
               return _teacherService.AddGradeToStudent(newGrade);
            });
        }

        [Fact]
        public async void AddGradeToStudent_SubjectsNoMatch()
        {
            var newGrade = new AddGradeToStudentVm()
            {
                TeacherId= 2,
                Grade = GradeScale.DB,
                StudentId = 6,
                SubjectId = 1
            };
            await Assert.ThrowsAsync<ArgumentException>(() =>
            {
               return _teacherService.AddGradeToStudent(newGrade);
            });
        }

        [Fact]
        public async void AddGradeToStudent_TeacherNoValue()
        {
            var newGrade = new AddGradeToStudentVm()
            {
                Grade = GradeScale.DB,
                StudentId = 6,
                SubjectId = 1
            };
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
            {
               return _teacherService.AddGradeToStudent(newGrade);
            });
        }
    }
}
