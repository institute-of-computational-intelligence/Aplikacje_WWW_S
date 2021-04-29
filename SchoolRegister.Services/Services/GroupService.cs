using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
        public GroupService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger) { }

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

      
    }
}