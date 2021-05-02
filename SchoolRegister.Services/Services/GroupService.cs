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
        public GroupService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger) 
        { 
            
        }
        public async void AddGroup(GroupVm groupVm)
        {
            if(string.IsNullOrEmpty(groupVm.Name))
            {
                throw new ArgumentNullException("Name cannot be empty");
            }

            var groupName = new Group() { Name = groupVm.Name };
            await DbContext.AddAsync(groupName);
            await DbContext.SaveChangesAsync();
        }
    }
} 