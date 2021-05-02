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
            var addstudent = new StudentVm()
            {
                StudentId = 8,
                GroupId = 3,
            };
            var st = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == addstudent.StudentId);
            _studentService.AddOrRemoveStudentGroup(addstudent);
            Assert.Equal(3,st.GroupId);
        }

        [Fact]
        public void RemoveStudent()
        {
            var removestudent = new StudentVm()
            {
                StudentId = 7,
                GroupId = 2,
            };
            var st = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == removestudent.StudentId);
            _studentService.AddOrRemoveStudentGroup(removestudent);
            Assert.Null(st.GroupId);
        }

        [Fact]
        public void AddRemoveStudentFromGroup_IvalidGroup()
        {
            var addstudent = new StudentVm()
            {
                StudentId = 7,
                GroupId = 33,
            };
            Assert.Throws<ArgumentNullException>(() =>
            {
                _studentService.AddOrRemoveStudentGroup(addstudent);
            });
        }

        [Fact]
        public void AddRemoveStudentIvalidStudent()
        {
            var addstudent = new StudentVm()
            {
                StudentId = 12,
                GroupId = 2,
            };
            Assert.Throws<ArgumentNullException>(() =>
            {
                _studentService.AddOrRemoveStudentGroup(addstudent);
            });
        }

        [Fact]
        public void AddRemoveStudentNoValues()
        {
            var addstudent = new StudentVm();
            Assert.Throws<ArgumentNullException>(() =>
            {
                _studentService.AddOrRemoveStudentGroup(addstudent);
            });
        }
    }
}