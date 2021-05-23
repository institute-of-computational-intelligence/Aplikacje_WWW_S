using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System.Threading.Tasks;

namespace SchoolRegister.Services.Services
{
    public class GroupService : BaseService, IGroupService
    {
        public GroupService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger)
        {

        }

        public async Task<GroupVm> AddGroupAsync(AddGroupVm addGroupVm)
        {
            if(string.IsNullOrEmpty(addGroupVm.Name))
            {
                throw new ArgumentNullException("Name value cannot be null or empty!");
            }

            var groupToBeAdded = new Group() { Name = addGroupVm.Name };

            var groupVm = Mapper.Map<GroupVm>(groupToBeAdded);
            await DbContext.AddAsync(groupToBeAdded);
            await DbContext.SaveChangesAsync();

            return groupVm;
        }

        public async Task<GroupVm> DeleteGroupAsync(DeleteGroupVm deleteGroupVm)
        {
            try
            {
                var groupToBeDeleted = await DbContext.Groups.FirstOrDefaultAsync(g => g.Id == deleteGroupVm.Id);    

                var groupVM=Mapper.Map<GroupVm>(groupToBeDeleted);
                if(groupToBeDeleted == null)
                {
                    throw new ArgumentNullException($"Could not find group with id: {deleteGroupVm.Id}");
                }

                DbContext.Groups.Remove(groupToBeDeleted);
                await DbContext.SaveChangesAsync();
                return groupVM;
            }
            catch(Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
} 