using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.Services.Services;
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
        public void Given_NullParameter_When_CallingAddToGroupAsync_Then_ThrowsArgumentNullException()
        {

            Assert.Throws<ArgumentNullException>(() => _studentService.AddToGroupAsync(null));         
        }

        [Fact]
        public void Given_NullParameter_When_CallingRemoveFromGroupAsync_Then_ThrowsArgumentNullException()
        {

            Assert.Throws<ArgumentNullException>(() => _studentService.RemoveFromGroupAsync(null));         
        }

        [Fact]
        public void Given_ValidParameter_When_CallingAddToGroupAsync_Then_AddingStudent()
        {
            var countBefore = DbContext.Users.Count();
            var getStudentToAdd = new AddToGroupVm()
            {
                GroupId = 1,
                StudentId = 1
            };
            
            _studentService.AddToGroupAsync(getStudentToAdd);

            var countAfter = DbContext.Users.Count();
            Assert.Equal(countAfter, countBefore);
        }

        [Fact]
        public void Given_ValidParameter_When_CallingRemoveFromGroupAsync_Then_RemovingStudent()
        {
            var getStudentToRemove = new RemoveFromGroupVm()
            {
                StudentId = 1
            };
                      
            _studentService.RemoveFromGroupAsync(getStudentToRemove);

            Assert.Null(DbContext.Users.OfType<Student>().First(x => x.Id == 2).GroupId);
        }      
    }
}