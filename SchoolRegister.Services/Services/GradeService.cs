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
    public class GradeService : BaseService, IGradeService
    {
        private readonly UserManager<User> userManager;
            public GradeService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger)
            {
            this.userManager = userManager;
             }
        public async Task<IEnumerable<Grade>> GetGrades(GetGradesVm getGradesVm)
        {
            
            try{
                var user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == getGradesVm.UserId);

                if(getGradesVm == null)

                    throw new ArgumentNullException ($"View model parameter is null");

                if(await userManager.IsInRoleAsync(user, "Parent") || await userManager.IsInRoleAsync(user, "Student"))
                {
                    var grades = DbContext.Grades.Where(g => g.StudentId == getGradesVm.StudentId).ToList();

                    return grades;
                }
                else
                    throw new ArgumentNullException ($"No permission");
                }catch (Exception ex) {
                     Logger.LogError (ex, ex.Message);
                throw;
            }
        }
    }
}