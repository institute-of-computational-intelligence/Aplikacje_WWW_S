using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.BLL.DataModels;
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
        public async void AddStudentToGroupUsingStudentId()
        {
            var countBefore = DbContext.Groups.First(x => x.Id == 1).Students.Count();

            var addStudentToGroup = new AddStudentToGroupVm()
            {
                StudentId = 10,
                GroupId = 1,
            };

            var updatedGroup = await _studentService.AddStudentToGroupAsync(addStudentToGroup);
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

            var addStudentToGroup = new AddStudentToGroupVm()
            {
                StudentId = 12, // User with Id 12 is teacher
                GroupId = 1,
            };

            // Should throw ArgumentNullException because user with Id 12 is not student
            await Assert.ThrowsAsync<ArgumentNullException>(() => _studentService.AddStudentToGroupAsync(addStudentToGroup));
            var countAfter = DbContext.Groups.First(x => x.Id == 1).Students.Count();

            Assert.Equal(countBefore, countAfter);
        }

        [Fact]
        public async void AddStudentToGroupUsingStudentIdAlreadyInGroup()
        {
            AddStudentToGroupUsingStudentId();

            var countBefore = DbContext.Groups.First(x => x.Id == 3).Students.Count();

            var addStudentToGroup = new AddStudentToGroupVm()
            {
                StudentId = 10,
                GroupId = 3,
            };

            await Assert.ThrowsAsync<DuplicateNameException>(() => _studentService.AddStudentToGroupAsync(addStudentToGroup));
            var countAfter = DbContext.Groups.First(x => x.Id == 3).Students.Count();

            Assert.Equal(countBefore, countAfter);
        }

        [Fact]
        public async void RemoveStudentFromGroup()
        {
            var countBefore = DbContext.Groups.First(x => x.Id == 3).Students.Count();

            var removeStudentFromGroup = new RemoveStudentFromGroupVm()
            {
                StudentId = 10,
                GroupId = 3,
            };

            var updatedGroup = await _studentService.RemoveStudentFromGroupAsync(removeStudentFromGroup);
            var countAfter = DbContext.Groups.First(x => x.Id == 3).Students.Count();

            Assert.NotNull(updatedGroup);
            Assert.DoesNotContain(10, DbContext.Groups.First(x => x.Id == 1).Students.Select(x => x.Id));
            Assert.Null(DbContext.Users.OfType<Student>().First(x => x.Id == 10).GroupId);
            Assert.True(countBefore > countAfter);
        }

        [Fact]
        public async void RemoveStudentFromGroupNotInGroup()
        {
            var countBefore = DbContext.Groups.First(x => x.Id == 1).Students.Count();

            var removeStudentFromGroup = new RemoveStudentFromGroupVm()
            {
                StudentId = 10,
                GroupId = 1,
            };

            var updatedGroup = await _studentService.RemoveStudentFromGroupAsync(removeStudentFromGroup);
            var countAfter = DbContext.Groups.First(x => x.Id == 1).Students.Count();

            Assert.Null(updatedGroup);
            Assert.Equal(countBefore, countAfter);
        }
    }
}
