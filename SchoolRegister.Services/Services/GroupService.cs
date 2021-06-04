using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
        public GroupService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper,
            logger)
        {
        }

        public async Task<GroupVm> AddGroupAsync(AddGroupVm addGroupVm)
        {
            try
            {
                var group = await DbContext.Groups.FirstOrDefaultAsync(g => g.Name == addGroupVm.Name);

                if (!(group is null))
                    throw new DuplicateNameException($"Group with name: {addGroupVm.Name} already exists");

                var newGroup = new Group { Name = addGroupVm.Name };
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

        public IEnumerable<GroupVm> GetGroups(Expression<Func<Group, bool>> filterExpressions = null)
        {
            try
            {
                var groupEntities = DbContext.Groups.AsQueryable();

                if (!(filterExpressions is null))
                    groupEntities = groupEntities.Where(filterExpressions);
                var groupVms = Mapper.Map<IEnumerable<GroupVm>>(groupEntities);
                return groupVms;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GroupVm> DeleteGroupAsync(RemoveGroupVm deleteGroupVm)
        {
            try
            {
                var group = await DbContext.Groups.FirstOrDefaultAsync(g => g.Id == deleteGroupVm.Id);

                if (group is null)
                    throw new ArgumentNullException($"Group with id: {deleteGroupVm.Id} does not exist");

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