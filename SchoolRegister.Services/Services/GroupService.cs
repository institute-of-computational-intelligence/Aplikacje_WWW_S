using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Linq.Expressions;


namespace SchoolRegister.Services.Services
{
    public class GroupService : BaseService, IGroupService
    {
        public GroupService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger) { }
        
        public void AddRemoveGroup(GroupVm groupVm)
        {
            try{
                if(groupVm == null)
                    throw new ArgumentNullException($"View model parameter is null");

                var groupEntity = Mapper.Map<Group>(groupVm);
                if(groupVm.Id == 0  || !groupVm.Id.HasValue)
                {
                    if(!string.IsNullOrEmpty(groupVm.Name))
                        DbContext.Groups.Add(groupEntity);
                    else
                        throw new ArgumentNullException("Please specify new group name!");
                }else{
                    if(!DbContext.Groups.Any(g => g.Id == groupVm.Id))
                        throw new ArgumentException("You can't remove non existing group!");
                    DbContext.Groups.Remove(groupEntity);  
                }
                DbContext.SaveChanges();
            }catch (Exception ex){
                Logger.LogError (ex, ex.Message);
                throw;
            }
        }

        public GroupVm GetGroup(Expression<Func<Group, bool>> filterExpression)
        {
            try
            {
                if(filterExpression == null)
                    throw new ArgumentNullException($" FilterExpression is null");
                var groupEntity = DbContext.Groups.FirstOrDefault(filterExpression);
                var groupVm = Mapper.Map<GroupVm>(groupEntity);
                return groupVm;
            }catch (Exception ex) {
                Logger.LogError (ex, ex.Message);
                throw;
            }
        }
        public IEnumerable<GroupVm> GetGroups(Expression<Func<Group, bool>> filterExpression = null)
        {
            try{
                var groupEntities = DbContext.Groups.AsQueryable();
                if(filterExpression != null)
                    groupEntities = groupEntities.Where(filterExpression);
                var groupVms = Mapper.Map<IEnumerable<GroupVm>>(groupEntities);
                return groupVms;
            }catch (Exception ex) {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}