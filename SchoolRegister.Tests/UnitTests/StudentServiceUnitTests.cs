using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;
namespace SchoolRegister.Tests.UnitTests
{
    public class StudentServiceUnitTests : BaseUnitTests
    {
        private readonly IStudentService _studentService;
        public StudentServiceUnitTests(ApplicationDbContext dbContext, IStudentService studentService) : base(dbContext)
        {
            _studentService = studentService;
        }
        [Fact]
        public void AddStudentToNewGroup()
        {
            var student = new StudentVm()
            {
                StudentId = 8,
                GroupId = 3,
            };
            var st = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == student.StudentId);
            _studentService.AddOrRemoveStudentGroup(student);
            Assert.Equal(3,st.GroupId);
        }

        [Fact]
        public void RemoveStudentFromGroup()
        {
            var student = new StudentVm()
            {
                StudentId = 7,
                GroupId = 2,
            };
            var st = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == student.StudentId);
            _studentService.AddOrRemoveStudentGroup(student);
            Assert.Null(st.GroupId);
        }

        [Fact]
        public void AddRemoveStudentFromGroup_IvalidGroup()
        {
            var student = new StudentVm()
            {
                StudentId = 7,
                GroupId = 33,
            };
            Assert.Throws<ArgumentNullException>(() =>
            {
                _studentService.AddOrRemoveStudentGroup(student);
            });
        }

        [Fact]
        public void AddRemoveStudentFromGroup_IvalidStudent()
        {
            var student = new StudentVm()
            {
                StudentId = 12,
                GroupId = 2,
            };
            Assert.Throws<ArgumentNullException>(() =>
            {
                _studentService.AddOrRemoveStudentGroup(student);
            });
        }

        [Fact]
        public void AddRemoveStudentFromGroup_NoValues()
        {
            var student = new StudentVm();
            Assert.Throws<ArgumentNullException>(() =>
            {
                _studentService.AddOrRemoveStudentGroup(student);
            });
        }
    }
}
