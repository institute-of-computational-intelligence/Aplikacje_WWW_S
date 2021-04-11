using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Services
{
    public class GroupService : BaseService, IGroupService
    {
        public GroupService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger)
        {
        }

        public async void AddGroupAsync(AddGroupVm addGroupVm)
        {
            try
            {
                if (addGroupVm == null)
                    throw new ArgumentNullException ($"View model parameter is null");

                Group group = await DbContext.Groups.FirstOrDefaultAsync(g => g.Name == addGroupVm.Name);

                if (!(group is null))
                    throw new ArgumentNullException ("Group with this name already exists");

                group = new Group() { Name = addGroupVm.Name };

                await DbContext.Groups.AddAsync(group);
                await DbContext.SaveChangesAsync();
            }catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async void DeleteGroupAsync(DeleteGroupVm deleteGroupVm)
        {
            try
            {
                 if (deleteGroupVm == null)
                    throw new ArgumentNullException ($"View model parameter is null");
                Group group = await DbContext.Groups.FirstOrDefaultAsync(g => g.Id == deleteGroupVm.Id);

                if (group is null)
                    throw new ArgumentNullException("Group with this id doesn't exist");

                DbContext.Groups.Remove(group);
                await DbContext.SaveChangesAsync();
            }catch (Exception exception)
            {
                Logger.LogError(exception.Message);
            }
        }
    }
}