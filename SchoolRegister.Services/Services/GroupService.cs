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
                if (groupVm is null)
                    throw new ArgumentNullException("View model parametr is missing");

                var group = Mapper.Map<Group>(groupVm);

                if (!groupVm.Id.HasValue || groupVm.Id == 0)
                {
                    if (String.IsNullOrEmpty(group.Name))
                        throw new ArgumentNullException("Provide valid group name.");
                    DbContext.Groups.Add(group);
                }
                else
                {
                    if (DbContext.Groups.Any(g => g.Id == groupVm.Id))
                        DbContext.Groups.Remove(group);
                    else
                        throw new ArgumentNullException("Group with specified Id does not exist.");
                }

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