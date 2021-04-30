using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;

namespace SchoolRegister.Tests.UnitTests
{
    public class StudentServiceUnitTest : BaseUnitTests
    {
        private readonly IStudentService _studentService;
        public StudentServiceUnitTest(ApplicationDbContext dbContext, IStudentService studentService) : base(dbContext)
        {
            _studentService = studentService;
        }

        [Fact]
        public void RemoveStudentFromGroup_RemoveStudnetTest()
        {
            var student = new StudentVm()
            {
                StudentId = 6,
                GroupId = 1
            };

            _studentService.RemoveStudentFromGroup(student);
            Assert.Equal(1, DbContext.Groups.FirstOrDefault(g => g.Id == student.GroupId).Students.Count);
        }

        [Fact]
        public void AddStudentToGroupTest_AddStudentTest()
        {
            var student = new StudentVm()
            {
                StudentId = 7,
                GroupId = 3
            };

            _studentService.AddStudentToGroup(student);
            Assert.Equal(3, DbContext.Groups.FirstOrDefault(g => g.Id == student.GroupId).Students.Count);
        }

        [Fact]
        public void RemoveStudentFromGroup_NullCheckTest()
        {
            Assert.Throws<ArgumentNullException>(() => _studentService.RemoveStudentFromGroup(null));
        }

        [Fact]
        public void RemoveStudentFromGroup_InvalidStudentTest()
        {
            var student = new StudentVm()
            {
                StudentId = 100,
                GroupId = 3
            };

            Assert.Throws<ArgumentNullException>(() => _studentService.RemoveStudentFromGroup(student));
        }

        [Fact]
        public void RemoveStudentFromGroup_InvalidGroupTest()
        {
            var student = new StudentVm()
            {
                StudentId = 7,
                GroupId = 333
            };

            Assert.Throws<ArgumentNullException>(() => _studentService.RemoveStudentFromGroup(student));
        }

        [Fact]
        public void RemoveStudentFromGroup_StudentNotInGroup()
        {
            var student = new StudentVm()
            {
                StudentId = 7,
                GroupId = 1
            };

            Assert.Throws<InvalidOperationException>(() => _studentService.RemoveStudentFromGroup(student));
        }

        [Fact]
        public void AddStudentToGroupTest_NullCheckTest()
        {
            Assert.Throws<ArgumentNullException>(() => _studentService.AddStudentToGroup(null));
        }

        [Fact]
        public void AddStudentToGroupTest_InvalidStudentTest()
        {
            var student = new StudentVm()
            {
                StudentId = 77,
                GroupId = 3
            };

            Assert.Throws<ArgumentNullException>(() => _studentService.AddStudentToGroup(student));

        }

        [Fact]
        public void AddStudentToGroupTest_InvalidGroupTest()
        {
            var student = new StudentVm()
            {
                StudentId = 7,
                GroupId = 322
            };

            Assert.Throws<ArgumentNullException>(() => _studentService.AddStudentToGroup(student));

        }

        [Fact]
        public void AddStudentToGroupTest_StudentAlreadyExistInGroupTest()
        {
            var student = new StudentVm()
            {
                StudentId = 5,
                GroupId = 1
            };

            Assert.Throws<InvalidOperationException>(() => _studentService.AddStudentToGroup(student));
        }


    }
}