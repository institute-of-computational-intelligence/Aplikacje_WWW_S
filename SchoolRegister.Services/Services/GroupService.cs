using System;
using System.Data;
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
            try 
            {
                Group group = await DbContext.Groups.FirstOrDefaultAsync(g => g.Name == addGroupVm.Name);

                if (!(group is null))
                    throw new DuplicateNameException($"Group with name: {addGroupVm.Name} already exists");

                var groupToBeAdded = new Group() { Name = addGroupVm.Name };
                var groupVm = Mapper.Map<GroupVm>(groupToBeAdded);

                await DbContext.AddAsync(groupToBeAdded);
                await DbContext.SaveChangesAsync();

                return groupVm;
            }
            catch(Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
           

            
        }

        public async Task<GroupVm> DeleteGroupAsync(DeleteGroupVm deleteGroupVm)
        {
            try
            {
                var groupToBeDeleted = await DbContext.Groups.FirstOrDefaultAsync(g => g.Id == deleteGroupVm.Id);    

                if(groupToBeDeleted == null)
                {
                    throw new ArgumentNullException($"Could not find group with id: {deleteGroupVm.Id}");
                }
                var groupVm = Mapper.Map<GroupVm>(groupToBeDeleted);

                DbContext.Groups.Remove(groupToBeDeleted);
                await DbContext.SaveChangesAsync();
                
                return groupVm;
            }
            catch(Exception exception)
            {
                Logger.LogError(exception.Message);
                throw;
            }
        }
    }
} 