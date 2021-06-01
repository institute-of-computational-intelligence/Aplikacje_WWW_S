using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;

namespace SchoolRegister.Tests.UnitTests
{
    public class GroupServiceUnitTest : BaseUnitTests
    {
        private readonly IGroupService _groupService;
        public GroupServiceUnitTest(ApplicationDbContext dbContext, IGroupService groupService) : base(dbContext)
        {
            _groupService = groupService;
        }

        [Fact]
        public void AddOrRemoveGroup_RemoveGroup()
        {
            var group = new GroupVm()
            {
                Id = 3,
                Name = ""
            };

            _groupService.AddOrRemoveGroup(group);
            Assert.Equal(2, DbContext.Groups.Count());
        }

        [Fact]
        public void AddOrRemoveGroup_AddGroup()
        {
            var group = new GroupVm()
            {
                Name = "Budowlanka"
            };

            _groupService.AddOrRemoveGroup(group);
            Assert.Equal(3, DbContext.Groups.Count());
        }

        [Fact]
        public void AddOrRemoveGroup_NullTest()
        {
            Assert.Throws<ArgumentNullException>(() => _groupService.AddOrRemoveGroup(null));
        }

        [Fact]
        public void AddOrRemoveGroup_EmptyGroupNameTest()
        {
            var group = new GroupVm()
            {
                Name = ""
            };

            Assert.Throws<ArgumentNullException>(() => _groupService.AddOrRemoveGroup(group));
        }

        [Fact]
        public void AddOrRemoveGroup_InvalidGroupIdTest()
        {
            var group = new GroupVm()
            {
                Id = 33
            };

            Assert.Throws<ArgumentNullException>(() => _groupService.AddOrRemoveGroup(group));
        }

    }
}