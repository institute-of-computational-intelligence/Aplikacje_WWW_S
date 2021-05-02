using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System;
using Microsoft.AspNetCore.Identity;

namespace SchoolRegister.Services.Services
{
    public class GroupService : BaseService, IGroupService
    {
        public GroupService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger) 
        { 
            
        }
        public void AddOrRemoveGroup(GroupVm groupVm)
        {
            try
            {
                if (groupVm == null)
                    throw new ArgumentNullException($"Viev model parameter is null");

                var groupEntity = Mapper.Map<Group>(groupVm);
                if (groupVm.Id != 0)
                {
                    DbContext.Groups.Add(groupEntity);
                }
                else
                {
                    DbContext.Groups.Remove(groupEntity);
                }
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
} 