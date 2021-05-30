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
        public async void AddStudentWithStudentId()
        {
            var countBefore = DbContext.Groups.First(x => x.Id == 1).Students.Count();
            var addStudentToGroup = new AddToGroupVm()
            {
                StudentId = 2,
                GroupId = 1,
            };

            var updatedGroup = await _studentService.AddToGroupAsync(addStudentToGroup);
            var countAfter = DbContext.Groups.First(x => x.Id == 1).Students.Count();

            Assert.NotNull(updatedGroup);
            Assert.Contains(10, DbContext.Groups.First(x => x.Id == 1).Students.Select(x => x.Id));
            Assert.Equal(1, DbContext.Users.OfType<Student>().First(x => x.Id == 10).GroupId);
            Assert.True(countBefore < countAfter);
        }

        [Fact]
        public async void AddStudentWithTeacherId()
        {
            var countBefore = DbContext.Groups.First(x => x.Id == 1).Students.Count();
            var addStudentToGroup = new AddToGroupVm()
            {
                StudentId = 12,
                GroupId = 1,
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() => _studentService.AddToGroupAsync(addStudentToGroup));
            var countAfter = DbContext.Groups.First(x => x.Id == 1).Students.Count();

            Assert.Equal(countBefore, countAfter);
        }

        [Fact]
        public async void AddStudentWithId_Group()
        {
            AddStudentWithStudentId();
            var countBefore = DbContext.Groups.First(x => x.Id == 3).Students.Count();
            var addStudentToGroup = new AddToGroupVm()
            {
                StudentId = 2,
                GroupId = 3,
            };

            await Assert.ThrowsAsync<DuplicateNameException>(() => _studentService.AddToGroupAsync(addStudentToGroup));
            var countAfter = DbContext.Groups.First(x => x.Id == 3).Students.Count();

            Assert.Equal(countBefore, countAfter);
        }

        [Fact]
        public async void RemoveStudent()
        {
            var countBefore = DbContext.Groups.First(x => x.Id == 3).Students.Count();
            var removeStudentFromGroup = new RemoveFromGroupVm()
            {
                StudentId = 2,
            };

            var updatedGroup = await _studentService.RemoveFromGroupAsync(removeStudentFromGroup);
            var countAfter = DbContext.Groups.First(x => x.Id == 3).Students.Count();

            Assert.NotNull(updatedGroup);
            Assert.DoesNotContain(10, DbContext.Groups.First(x => x.Id == 1).Students.Select(x => x.Id));
            Assert.Null(DbContext.Users.OfType<Student>().First(x => x.Id == 10).GroupId);
            Assert.True(countBefore > countAfter);
        }

        [Fact]
        public async void RemoveStudent_NotInGroup()
        {
            var countBefore = DbContext.Groups.First(x => x.Id == 1).Students.Count();
            var removeStudentFromGroup = new RemoveFromGroupVm()
            {
                StudentId = 2,
            };

            var updatedGroup = await _studentService.RemoveFromGroupAsync(removeStudentFromGroup);
            var countAfter = DbContext.Groups.First(x => x.Id == 1).Students.Count();

            Assert.Null(updatedGroup);
            Assert.Equal(countBefore, countAfter);
        }
    }
}