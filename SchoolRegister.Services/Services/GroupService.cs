using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Identity;




namespace SchoolRegister.Services.Services
{
    public class GroupService : BaseService, IGroupService
    {
        public GroupService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger)
        {
        }

        public void AddGroup(GroupVm groupVm)
        {
            try
            {
                if (groupVm is null)
                    throw new ArgumentNullException($"View model parametr is missing");

                var group = Mapper.Map<Group>(groupVm);

                if (DbContext.Groups.Any(g => g.Id == groupVm.Id))
                    throw new ArgumentException("This group already exist");
                else
                    DbContext.Groups.Add(group);
                DbContext.SaveChanges();

            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                throw;
            }
        }
        public void DeleteGroup(GroupVm groupVm)
        {
            try
            {
                if (groupVm is null)
                    throw new ArgumentNullException($"View model parametr is missing");

                var group = Mapper.Map<Group>(groupVm);

                if (DbContext.Groups.Any(g => g.Id == groupVm.Id))
                    DbContext.Groups.Remove(group);
                else
                    throw new ArgumentException("This group doesn't exist");
                DbContext.SaveChanges();

            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                throw;
            }
        }

    }
}