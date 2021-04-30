using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System.Threading.Tasks;

namespace SchoolRegister.Services.Services
{
    public class GroupService : BaseService, IGroupService
    {
        public GroupService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger){
        }

        public async Task<GroupVm> AddGroupAsync(AddGroupVm addGroupVm)
        {
            try
            {
                if (addGroupVm == null)
                    throw new ArgumentNullException ($"View model parameter is null");

                Group group = await DbContext.Groups.FirstOrDefaultAsync(g => g.Name == addGroupVm.Name);

                if (!(group is null))
                    throw new ArgumentNullException ("Group with this name already exists");

                group = new Group() { Name = addGroupVm.Name };
                var groupVm = Mapper.Map<GroupVm>(group);
                await DbContext.Groups.AddAsync(group);
                await DbContext.SaveChangesAsync();
                return groupVm;
            }catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GroupVm> DeleteGroupAsync(DeleteGroupVm deleteGroupVm)
        {
            try
            {
                 if (deleteGroupVm == null)
                    throw new ArgumentNullException ($"View model parameter is null");
                Group group = await DbContext.Groups.FirstOrDefaultAsync(g => g.Id == deleteGroupVm.Id);
                var groupVm = Mapper.Map<GroupVm>(group);
                if (group is null)
                    throw new ArgumentNullException("Group with this id doesn't exist");

                DbContext.Groups.Remove(group);
                await DbContext.SaveChangesAsync();
                return groupVm;
            }catch (Exception exception)
            {
                Logger.LogError(exception.Message);
                throw;
            }
        }
    }
}