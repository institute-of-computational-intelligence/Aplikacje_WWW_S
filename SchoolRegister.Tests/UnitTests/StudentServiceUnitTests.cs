using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;
using System.Data;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.Tests.UnitTests
{
    public class StudentServiceUnitTests : BaseUnitTests
    {
        private readonly IStudentService _studentService;

        public StudentServiceUnitTests(ApplicationDbContext dbContext, IStudentService studentService) : base(dbContext){
            _studentService = studentService;
        }

        [Fact]
        public async void AddStudentToGroup(){
            var addStudentToGroup = new AddStudentToGroupVm()
            {
                StudentId = 2,
                GroupId = 2,
            };

            var updatedGroup = await _studentService.AddStudentToGroupAsync(addStudentToGroup);

            Assert.NotNull(updatedGroup);
            Assert.Contains(2, DbContext.Groups.First(x => x.Id == 2).Students.Select(x => x.Id));
            Assert.Equal(2, DbContext.Users.OfType<Student>().First(x => x.Id == 2).GroupId);
        }

        [Fact]
        public async void AddStudentToGroupAlreadyAdded(){
            AddStudentToGroup();
            var addStudentToGroup = new AddStudentToGroupVm()
            {
                StudentId = 2,
                GroupId = 2,
            };

            await Assert.ThrowsAsync<DuplicateNameException>(() => _studentService.AddStudentToGroupAsync(addStudentToGroup));
        }

        [Fact]
        public async void RemoveStudentFromGroup(){
            var removeStudentFromGroup = new RemoveStudentFromGroupVm()
            {
                StudentId = 2,
                GroupId = 2,
            };

            var groupAfter = await _studentService.RemoveStudentFromGroupAsync(removeStudentFromGroup);
        
            Assert.DoesNotContain(2, DbContext.Groups.First(x => x.Id == 2).Students.Select(x => x.Id));
            Assert.Null(DbContext.Users.OfType<Student>().First(x => x.Id == 2).GroupId);
        }

        [Fact]
        public async void RemoveStudentFromGroupWithoutStudent(){
            var removeStudentFromGroup = new RemoveStudentFromGroupVm()
            {
                StudentId = 2,
                GroupId = 2,
            };

            var groupAfter = await _studentService.RemoveStudentFromGroupAsync(removeStudentFromGroup);

            Assert.Null(groupAfter);
        }
    }
}