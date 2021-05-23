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
    /*
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
            var student = new StudentVm()
            {
                StudentId = 8,
                GroupId = 3,
            };
            var st = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == student.StudentId);
            _studentService.AddStudentToGroup(student);
            Assert.Equal(3, st.GroupId);
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
            _studentService.RemoveStudentFromGroup(student);
            Assert.Null(st.GroupId);
        }
    }*/
}
