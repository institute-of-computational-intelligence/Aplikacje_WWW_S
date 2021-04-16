  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.InterFaces;
using SchoolRegister.ViewModels.VM;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
namespace SchoolRegister.Services.Services
{

    public class GroupService : BaseService, IGroupService
    {
        public GroupService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger)
        {


        }
        public async void AddGroup(GroupVm addGroupVm)
        {

            try{
                if(addGroupVm == null)
                    throw new ArgumentNullException ($"View model parameter is null");

                    Group group = new Group() {Name = addGroupVm.Name};
                
                    await DbContext.Groups.AddAsync(group);
                     await DbContext.SaveChangesAsync();

            }catch (Exception ex) {
                Logger.LogError (ex, ex.Message);
                throw;
            }
        }

        public async void DeleteGroup(GroupVm deleteGroupVm)
        {
                try{
                    if(deleteGroupVm == null)
                      throw new ArgumentNullException ($"View model parameter is null");

                         Group group = new Group() {Name = deleteGroupVm.Name};
                         DbContext.Groups.Remove(group);
                         await DbContext.SaveChangesAsync();

                        }catch (Exception ex) {
                         Logger.LogError (ex, ex.Message);
                        throw;
            }
        }
    }
}