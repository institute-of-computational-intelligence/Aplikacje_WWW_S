using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;

namespace SchoolRegister.Tests.UnitTests
{
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
                    Name = "PAI"
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



            [Fact]
            public void Remove()
            {
                var group = new AddGroupVm()
                {
                    Name = "PAI"
                };

                try
                {
                    _groupService.AddGroup(group);
                    Assert.True(true);
                }
                catch
                {
                    Assert.True(false);
                }

            }


        }
    }
}
