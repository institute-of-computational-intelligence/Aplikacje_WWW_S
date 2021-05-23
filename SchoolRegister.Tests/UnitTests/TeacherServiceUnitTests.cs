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
        public async void AddGradeError()
        {
            var addGradeToStudent = new AddGradeAsyncVm()
            {
                StudentId = 4,
                SubjectId = 33,
                GradeValue = GradeScale.DB
            };
            await Assert.ThrowsAsync<ArgumentNullException>(() => _teacherService.AddGradeAsync(addGradeToStudent));
        }
    }
}