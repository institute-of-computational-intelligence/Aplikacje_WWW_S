using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRegister.Services.Services
{
    public class GroupService : BaseService, IGroupService
    {
        public GroupService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger, userManager)
        {
        }

        public void AddOrRemoveGroup(GroupVm groupVm)
        {
            try
            {
                if (groupVm is null || !groupVm.Id.HasValue || groupVm.Id == 0)
                    throw new ArgumentNullException("View model parametr is missing");

                var group = Mapper.Map<Group>(groupVm);

                if (DbContext.Groups.Any(g => g.Id == groupVm.Id))
                    DbContext.Groups.Remove(group);
                else
                    DbContext.Groups.Add(group);

                DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}