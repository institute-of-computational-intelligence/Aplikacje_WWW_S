using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    public class GradeService : BaseService, IGradeService
    {
        private readonly UserManager<User> _userManager;

        public GradeService(UserManager<User> userManager, ApplicationDbContext dbContext, IMapper mapper, ILogger logger) 
            : base(dbContext, mapper, logger)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<Grade>> GetGradesAsync(GetGradesVm getGradesVm)
        {
            try
            {
                User student = await DbContext.Users
                    .FirstOrDefaultAsync(u => u.Id == getGradesVm.StudentId);

                User user = await DbContext.Users
                    .FirstOrDefaultAsync(u => u.Id == getGradesVm.UserId);

                if (student is null)
                    throw new ArgumentNullException($"User with id: {getGradesVm.StudentId} does not exist");

                if (!(await _userManager.IsInRoleAsync(user, "Parent") || await _userManager.IsInRoleAsync(user, "Student")))
                    throw new UnauthorizedAccessException("Insufficient permissions, only student and parent can access the grades");

                List<Grade> grades = DbContext.Grades.Where(g => g.StudentId == getGradesVm.StudentId).ToList();

                return grades;
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message);
                throw;
            }
        }
        
        // public async Task<IEnumerable<Grade>> GetGrades(GetGradesVm getGradesVm)
        // {
        //     try
        //     {
        //         var user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == getGradesVm.UserId);
        //         if(getGradesVm == null)
        //             throw new ArgumentNullException ($"View model parameter is null");
        //         /*
        //         if(await _userManager.IsInRoleAsync(user, "Parent") || await _userManager.IsInRoleAsync(user, "Student"))
        //         {
        //             var grades = DbContext.Grades.Where(g => g.StudentId == getGradesVm.StudentId).ToList();
        //             return grades;
        //         }
        //         else
        //             throw new ArgumentNullException ($"No permissions");
        //         */
        //         var grades = DbContext.Grades.Where(g => g.StudentId == getGradesVm.StudentId).ToList();
        //         return grades;
        //     } catch (Exception ex) {
        //         Logger.LogError (ex, ex.Message);  
        //         throw;
        //     }
        // }
        public async Task<IEnumerable<GradeVm>> GetGrades(GetGradesVm getGradesVm)
        {
            try
            {
                var gradesEntities = DbContext.Grades.Where(g => g.StudentId == getGradesVm.StudentId).AsQueryable();
                if(getGradesVm == null)
                    throw new ArgumentNullException ($"View model parameter is null");
                var gradesVms = Mapper.Map<IEnumerable<GradeVm>>(gradesEntities);
                return gradesVms;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}