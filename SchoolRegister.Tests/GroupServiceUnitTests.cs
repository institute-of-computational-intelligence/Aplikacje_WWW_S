using SchoolRegister.DAL.EntityFramework;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System;
using System.Data;
using System.Linq;
using Xunit;

namespace SchoolRegister.Tests
{
    public class GroupServiceUnitTests : BaseUnitTests
    {
        private readonly IGroupService _groupService;

        public GroupServiceUnitTests(ApplicationDbContext dbContext, IGroupService groupService) : base(dbContext)
        {
            _groupService = groupService;
        }

        [Fact]
        public async void AddGroup()
        {
            var countBefore = DbContext.Groups.Count();

            var addGroup = new AddGroupVm()
            {
                Name = "SK",
            };

            var group = await _groupService.AddGroupAsync(addGroup);
            var countAfter = DbContext.Groups.Count();

            Assert.NotNull(group);
            Assert.Contains("SK", DbContext.Groups.Select(x => x.Name));
            Assert.True(countAfter > countBefore);
        }

        [Fact]
        public async void AddGroupAlreadyExists()
        {
            var countBefore = DbContext.Groups.Count();

            var addGroup = new AddGroupVm()
            {
                Name = "IO",
            };

            await Assert.ThrowsAsync<DuplicateNameException>(() => _groupService.AddGroupAsync(addGroup));
            var countAfter = DbContext.Groups.Count();

            Assert.Equal(countAfter, countBefore);
        }

        [Fact]
        public async void DeleteGroup()
        {
            var countBefore = DbContext.Groups.Count();

            var removeGroup = new RemoveGroupVm()
            {
                Id = 2,
            };

            var group = await _groupService.DeleteGroupAsync(removeGroup);
            var countAfter = DbContext.Groups.Count();

            Assert.DoesNotContain(2, DbContext.Groups.Select(x => x.Id));
            Assert.True(countAfter < countBefore);
        }

        [Fact]
        public async void DeleteGroupNonExisting()
        {
            var countBefore = DbContext.Groups.Count();

            var removeGroup = new RemoveGroupVm()
            {
                Id = 4,
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() => _groupService.DeleteGroupAsync(removeGroup));
            var countAfter = DbContext.Groups.Count();
            Assert.Equal(countAfter, countBefore);
        }
    }
}
