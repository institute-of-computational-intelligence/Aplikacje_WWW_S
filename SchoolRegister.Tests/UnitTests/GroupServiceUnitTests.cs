using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;
using System.Data;

namespace SchoolRegister.Tests.UnitTests 
{

    public class GroupServiceUnitTests: BaseUnitTests 
    {
        private readonly IGroupService _groupService;

        public GroupServiceUnitTests(ApplicationDbContext dbContext, IGroupService groupService): base(dbContext) 
        {
            _groupService = groupService;
        }

        [Fact]
        public async void AddGroup() 
        {
            var countBefore = DbContext.Groups.Count();

            var addGroup = new AddGroupVm() {
                Name = "SK",
            };

            await _groupService.AddGroupAsync(addGroup);
            var countAfter = DbContext.Groups.Count();

            Assert.NotNull(addGroup);
            Assert.Contains("SK", DbContext.Groups.Select(x => x.Name));
            Assert.True(countAfter > countBefore);
        }
        [Fact]
        public async void DeleteGroup() 
        {
            var countBefore = DbContext.Groups.Count();

            var deleteGroup = new DeleteGroupVm() {
                Id = 2,
            };

            var group = await _groupService.DeleteGroupAsync(deleteGroup);
            var countAfter = DbContext.Groups.Count();

            Assert.DoesNotContain(2, DbContext.Groups.Select(x => x.Id));
            Assert.True(countAfter < countBefore);
        }
    }
}