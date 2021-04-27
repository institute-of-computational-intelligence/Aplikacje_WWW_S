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
        public void Given_NullParameter_When_CallingAddGroupAsync_Then_ThrowsArgumentNullException()
        {

            Assert.Throws<ArgumentNullException>(() => _groupService.AddGroupAsync(null));         
        }

        [Fact]
        public void Given_NullParameter_When_CallingDeleteGroupAsync_Then_ThrowsArgumentNullException()
        {

            Assert.Throws<ArgumentNullException>(() => _groupService.DeleteGroupAsync(null));         
        }

        // do poprawy po uzyksaniu info 
        [Fact]
        public void Given_ValidParameter_When_CallingDeleteGroupAsync_Then_DeletingGroup()
        {
            var getGroupToDelete = new DeleteGroupVm()
            {
                Id = 1
            };
            
            _groupService.DeleteGroupAsync(getGroupToDelete);
        }

        [Fact]
        public void Given_ValidParameter_When_CallingAddGroupAsync_Then_AddingGroup()
        {
            var getGroupToAdd = new AddGroupVm()
            {
                Name = "Biol-Chem"
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
    }
}