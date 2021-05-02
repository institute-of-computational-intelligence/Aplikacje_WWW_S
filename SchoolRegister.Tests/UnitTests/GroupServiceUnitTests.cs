using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using SchoolRegister.Tests;
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
        public async void AddGroupTest()
        {
            int groupCount=DbContext.Groups.Count();
            var group=new GroupVm(){
                Id=4,
                Name="Group4"
            };
            _groupService.AddGroup(group);
            Assert.Equal(groupCount+1, DbContext.Groups.Count());
        }
        [Fact]
        public void DeleteGroupTest()
        {
            int groupCount=DbContext.Groups.Count();
            var group=new  GroupVm(){
                Id=3,
                Name=""
            };
            _groupService.DeleteGroup(group);
            Assert.Equal(groupCount-1, DbContext.Groups.Count());
        }
        
 
    }
}