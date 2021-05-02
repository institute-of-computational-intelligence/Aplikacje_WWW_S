using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.Services.Services;
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
        public void AddGroupValidParameter()
        {
            var getGroupToAdd = new GroupVm()
            {
                Name = "Io"
            };
            try
            {
                _groupService.AddGroup(getGroupToAdd);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }

        }
    }
} 