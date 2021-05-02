
using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;
using System.Linq.Expressions;
using System.Data;

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
        public void AddStudentToGroup()
        {
            var student = new AddStudentToGroupVm()
            {
                StudentId = 7,
                GroupId = 3,
            };
            var st = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == student.StudentId);
            _studentService.AddStudentToGroupAsync(student);
            Assert.Equal(3, st.GroupId);
        }

        /*
        [Fact]
        public void RemoveStudentFromGroup()
        {
            var student = new RemoveStudentFromGroupVm()
            {
                StudentId = 7,
                GroupId = 2
            };
            var st = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == student.StudentId);
            _studentService.RemoveStudentFromGroupAsync(student);
            Assert.Null(st.GroupId);
        }*/
    }
}