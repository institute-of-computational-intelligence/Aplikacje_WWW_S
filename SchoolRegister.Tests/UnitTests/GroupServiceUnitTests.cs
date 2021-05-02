using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;
namespace SchoolRegister.Tests.UnitTests
{
    public class GroupServiceUnitTests : BaseUnitTests
    {
        private readonly IGroupService _groupService;
        public GroupServiceUnitTests(ApplicationDbContext dbContext, IGroupService groupService) : base(dbContext)
        {
            _groupService = groupService;
        }
        /*[Fact]
        public void AddGradeToStudent()
        {
            var gradeVm = new AddGradeToStudentVm()
            {
                StudentId = 5,
                SubjectId = 1,
                Grade = GradeScale.DB,
                TeacherId = 1
            };
            var grade = _gradeService.AddGradeToStudent(gradeVm);
            Assert.NotNull(grade);
            Assert.Equal(2, DbContext.Grades.Count());
        }*/

        /*[Fact]
        public void AddGroup()
        {
            var addGroup = new AddGroupVm()
            {
                Name = "Informatyka stosowana",
            };
            _groupService.AddGroupAsync(addGroup);
        }*/
        [Fact]
        public void AddGroup()
        {
            var getGroupToAdd = new AddGroupVm()
            {
                Name = "Informatyka stosowana"
            };

            try
            {
                _groupService.AddGroupAsync(getGroupToAdd);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }

        }
        [Fact]
        public void DeleteGroup()
        {
            var getGroupToDelete = new RemoveGroupVm()
            {
                Id = 1
            };

            _groupService.DeleteGroupAsync(getGroupToDelete);
        }
    }
}