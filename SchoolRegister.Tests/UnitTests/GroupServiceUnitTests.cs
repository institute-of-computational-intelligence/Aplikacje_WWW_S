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
        [Fact]
        public void AddNewGroup()
        {
            var group = new GroupVm()
            {
                Id = 0,
                Name = "Erasmus+"
            };
            _groupService.AddRemoveGroup(group);
            Assert.Equal(5,DbContext.Groups.Count());
        }

        [Fact]
        public void AddNewGroupNoId()
        {
            var group = new GroupVm()
            {
                Name = "Angielski B1"
            };
            _groupService.AddRemoveGroup(group);
            Assert.Equal(4,DbContext.Groups.Count());
        }
 
        [Fact]
        public void RemoveExistingGroup()
        {
            var group = new GroupVm()
            {
                Id = 1
            };
            _groupService.AddRemoveGroup(group);
            Assert.Equal(4,DbContext.Groups.Count());
        }

        [Fact]
        public void AddNewGroup_NoGroupName()
        {
            var group = new GroupVm()
            {
                Id = 0
            };

            Assert.Throws<ArgumentNullException>(() =>
            {
                _groupService.AddRemoveGroup(group);
            });
        }

        [Fact]
        public void RemoveGroup_NoExistingGroup()
        {
            var group = new GroupVm()
            {
                Id = 20,
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _groupService.AddRemoveGroup(group);
            });
        }

        [Fact]
        public void AddRemoveGroup_NoValues()
        {
            var group = new GroupVm();
            
            Assert.Throws<ArgumentNullException>(() =>
            {
                _groupService.AddRemoveGroup(group);
            });
        }

    }
}
