using System;
using System.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.BLL.DataModels;
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

        public async void AddGroupAsync(AddGroupVm addGroupVm)
        {
            try
            {

                Group group = await DbContext.Groups.FirstOrDefaultAsync(g => g.Name == addGroupVm.Name);

                if (!(group is null))
                    throw new DuplicateNameException($"Group with name: {addGroupVm.Name} already exists");

                Group newGroup = new Group() { Name = addGroupVm.Name };

                await DbContext.Groups.AddAsync(newGroup);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async void DeleteGroupAsync(RemoveGroupVm deleteGroupVm)
        {
            try
            {
                Group group = await DbContext.Groups.FirstOrDefaultAsync(g => g.Id == deleteGroupVm.Id);

                if (group is null)
                    throw new ArgumentNullException($"Group with id: {deleteGroupVm.Id} does not exist");

                DbContext.Groups.Remove(group);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message);
            }
        }
    }
}