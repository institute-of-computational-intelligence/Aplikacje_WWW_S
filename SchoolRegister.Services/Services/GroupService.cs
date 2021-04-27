using System;
using System.Data;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.Model.DataModels;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

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
                    throw new DuplicateNameException("Nazwa nie może być pusta");

                Group newGroup = new Group() { Name = addGroupVm.Name };
                var groupVm = Mapper.Map<GroupVm>(newGroup);

                await DbContext.Groups.AddAsync(newGroup);
                await DbContext.SaveChangesAsync();

                return groupVm;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GroupVm> DeleteGroupAsync(DeleteGroupVm deleteGroupVm)
        {
            try
            {
                var group = await DbContext.Groups.FirstOrDefaultAsync(g => g.Id == deleteGroupVm.Id);

                if (group is null)
                    throw new ArgumentNullException("Grupa nie istnieje");

                var groupVm = Mapper.Map<GroupVm>(group);

                DbContext.Groups.Remove(group);
                await DbContext.SaveChangesAsync();

                return groupVm;
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message);
                throw;
            }
        }
    }
}