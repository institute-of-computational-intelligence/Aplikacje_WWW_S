
using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;
using System.Linq.Expressions;
using System.Data;
using SchoolRegister.Model.DataModels;

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
        public async void AddStudentToGroupUsingStudentId()
        {
            var countBefore = DbContext.Groups.First(x => x.Id == 1).Students.Count();

            var addToGroup = new AddToGroupVm()
            {
                StudentId = 10,
                GroupId = 1,
            };
            var updatedGroup = await _studentService.AddToGroupAsync(addToGroup);
            var countAfter = DbContext.Groups.First(x => x.Id == 1).Students.Count();

            Assert.NotNull(updatedGroup);
            Assert.Contains(10, DbContext.Groups.First(x => x.Id == 1).Students.Select(x => x.Id));
            Assert.Equal(1, DbContext.Users.OfType<Student>().First(x => x.Id == 10).GroupId);
            Assert.True(countBefore < countAfter);
        }
         [Fact]
        public async void AddStudentToGroupUsingTeacherId()
        {
            var countBefore = DbContext.Groups.First(x => x.Id == 1).Students.Count();

            var addStudentToGroup = new AddToGroupVm()
            {
                StudentId = 12, // User with Id 12 is teacher
                GroupId = 1,
            };

            // Should throw ArgumentNullException because user with Id 12 is not student
            await Assert.ThrowsAsync<ArgumentNullException>(() => _studentService.AddToGroupAsync(addStudentToGroup));
            var countAfter = DbContext.Groups.First(x => x.Id == 1).Students.Count();

            Assert.Equal(countBefore, countAfter);
        }
         [Fact]
        public async void RemoveStudentFromGroup()
        {
            var countBefore = DbContext.Groups.First(x => x.Id == 3).Students.Count();

            var removeStudentFromGroup = new RemoveFromGroupVm()
            {
                StudentId = 10,
                GroupId = 3,
            };

            var updatedGroup = await _studentService.RemoveFromGroupAsync(removeStudentFromGroup);
            var countAfter = DbContext.Groups.First(x => x.Id == 3).Students.Count();

            Assert.NotNull(updatedGroup);
            Assert.DoesNotContain(10, DbContext.Groups.First(x => x.Id == 1).Students.Select(x => x.Id));
            Assert.Null(DbContext.Users.OfType<Student>().First(x => x.Id == 10).GroupId);
            Assert.True(countBefore > countAfter);
        }
    }
}