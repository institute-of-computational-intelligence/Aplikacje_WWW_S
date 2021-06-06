using System;
using System.Data;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;

namespace SchoolRegister.Tests.UnitTests
{
    public class GroupServiceUnitTests : BaseUnitTests{
        private readonly IGroupService _groupService;

        public GroupServiceUnitTests(ApplicationDbContext dbContext, IGroupService groupService) : base(dbContext)
        {
            _groupService = groupService;
        }

        [Fact]
        public async void AddGroup()
        {
            var addGroup = new AddUpdateGroupVm()
            {
                Name = "Grupa 1",
            };

            var group = await _groupService.AddGroupAsync(addGroup);

            Assert.NotNull(group);
            Assert.Contains("Grupa 1", DbContext.Groups.Select(x => x.Name));
        }

        [Fact]
        public async void DeleteGroup()
        {
            var deleteGroup = new DeleteGroupVm()
            {
                Id = 3,
            };

            var group = await _groupService.DeleteGroupAsync(deleteGroup);
            Assert.DoesNotContain(3, DbContext.Groups.Select(x => x.Id));
        }

        [Fact]
        public async void DeleteGroupError() {
            var deleteGroup = new DeleteGroupVm()
            {
                Id = 45,
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() => _groupService.DeleteGroupAsync(deleteGroup));
        }
    }
}