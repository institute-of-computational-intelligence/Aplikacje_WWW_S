using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using SchoolRegister.Tests;
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
        public async void AddGradeToStudentByTeacher()
        {
            var countBefore = DbContext.Grades.Count();
            var addGradeTest=new AddGradeVm()
            {
                StudentId=5,
                SubjectId=2,
                GradeValue=GradeScale.DB,
                TeacherId=2
            };
            await _teacherService.AddGrade(addGradeTest);
            var countAfter=DbContext.Grades.Count();
            Assert.Equal(countBefore+1,countAfter);
        }
        [Fact]
        public async void AddGradeToStudentByStudent()
        {
            var countBefore = DbContext.Grades.Count();
            var addGradeTest=new AddGradeVm()
            {
                StudentId=5,
                SubjectId=2,
                GradeValue=GradeScale.DB,
                TeacherId=5
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() => _teacherService.AddGrade(addGradeTest));

        }

        [Fact]
        public async void SendEmailByTeacher()
        {
            var emailTest = new SendEMailVm()
            {
                TeacherId = 2,
                ParentId = 4,
                Title = "Test",
                Content = "."
            };

            var exception = await Record.ExceptionAsync(() => Task.Run(() => _teacherService.SendEMail(emailTest)));

            Assert.Null(exception);
        }
        
    }
}