using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;


namespace SchoolRegister.Services.Services
{
    public class GroupService : BaseService, IGroupService
    {
        public GroupService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger)
        {

        }

        public async void AddGroupAsync(AddGroupVm addGroupVm)
        {
            if (string.IsNullOrEmpty(addGroupVm.Name))
            {
                throw new ArgumentNullException("Name value cannot be null or empty!");
            }

            var groupToBeAdded = new Group() { Name = addGroupVm.Name };

            await DbContext.AddAsync(groupToBeAdded);
            await DbContext.SaveChangesAsync();
        }

        public async void DeleteGroupAsync(DeleteGroupVm deleteGroupVm)
        {
            try
            {
                var groupToBeDelted = DbContext.Groups.FirstOrDefault(g => g.Id == deleteGroupVm.Id);

                if (groupToBeDelted == null)
                {
                    throw new ArgumentNullException($"Could not find group with id: {deleteGroupVm.Id}");
                }

                DbContext.Groups.Remove(groupToBeDelted);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message);
            }
        }
    }
}