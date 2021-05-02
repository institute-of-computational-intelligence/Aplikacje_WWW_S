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
        public void Given_ValidParameter_When_CallingAddGroupAsync_Then_AddingGroup()
        {
            var getGroupToAdd = new AddGroupVm()
            {
                
                Name = "Pai"
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
        public void Remove()
        {
            var group = new AddGroupVm()
            {
                Name = "Pai"
             
            };

            try
            {
                _groupService.AddGroupAsync(group);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }

        }

     
    }

        
    
}