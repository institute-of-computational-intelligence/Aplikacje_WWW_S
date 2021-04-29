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

        public GroupServiceUnitTests(ApplicationDbContext dbContext, IGroupService groupService) : base(dbContext){
            _groupService = groupService;
        }

        [Fact]
        public async void AddGroup(){
            var addGroup = new AddGroupVm()
            {
                Name = "PAI",
            };

            var group = await _groupService.AddGroupAsync(addGroup);

            Assert.NotNull(group);
            Assert.Contains("PAI", DbContext.Groups.Select(x => x.Name));
        }

        [Fact]
        public async void AddGroupAlreadyExists(){
            AddGroup();
            var addGroup = new AddGroupVm()
            {
                Name = "PAI",
            };

            await Assert.ThrowsAsync<DuplicateNameException>(() => _groupService.AddGroupAsync(addGroup));
        }

        [Fact]
        public async void DeleteGroup(){
            var removeGroup = new DeleteGroupVm()
            {
                Id = 1,
            };

            var group = await _groupService.DeleteGroupAsync(removeGroup);
            Assert.DoesNotContain(2, DbContext.Groups.Select(x => x.Id));
        }

        [Fact]
        public async void DeleteGroupNonExisting() {
            var countBefore = DbContext.Groups.Count();

            var removeGroup = new DeleteGroupVm()
            {
                Id = 20,
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() => _groupService.DeleteGroupAsync(removeGroup));
        }
    }
}