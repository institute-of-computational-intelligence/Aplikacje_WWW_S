using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                StudentId = 4,
                GroupId = 6,
            };
            var st = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == student.StudentId);
            _studentService.AddOrRemoveStudentGroup(student);
            Assert.Equal(3, st.GroupId);
        }

        [Fact]
        public void RemoveStudentFromGroup()
        {
            var student = new StudentVm()
            {
                StudentId = 5,
                GroupId = 4,
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
                StudentId = 2,
                GroupId = 3,
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
                StudentId = 1,
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