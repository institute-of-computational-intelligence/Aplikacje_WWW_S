using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.Model.DataModels;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Services
{
    public class ParentService : BaseService, IParentService
    {
        public ParentService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger)
        {
            
        }

        public async Task<ParentVm> GetParentAsync(Expression<Func<Parent, bool>> filterExpressions)
        {
            try
            {
                if (filterExpressions == null)
                    throw new ArgumentNullException("filterExpressions is null");

                var parentEntity = await DbContext.Users.OfType<Parent>().FirstOrDefaultAsync(filterExpressions);

                var parentVm = Mapper.Map<ParentVm>(parentEntity);
                return parentVm;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<ParentVm> GetParents(Expression<Func<Parent, bool>> filterExpressions = null)
        {
            try
            {
                var parentsEntities = DbContext.Users.OfType<Parent>().AsQueryable();
                if (filterExpressions != null)
                    parentsEntities = parentsEntities.Where(filterExpressions);
                var parentsVms = Mapper.Map<IEnumerable<ParentVm>>(parentsEntities);
                return parentsVms;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}