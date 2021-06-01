using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;
using System.Data;
using System.Threading.Tasks;

namespace SchoolRegister.Tests.UnitTests
{
    public class GroupServiceUnitTests : BaseUnitTests
    {
        private readonly IGroupService _groupService;

        public GroupServiceUnitTests(ApplicationDbContext dbContext, IGroupService groupService) : base(dbContext)
        {
            _groupService = groupService;
        }

        



    }
}