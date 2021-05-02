using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Services
{
    public class GradeService : BaseService, IGradeService
    {
        private readonly UserManager<User> userManager;

        public GradeService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger)
        {
            this.userManager = userManager;
        }
        public async Task<IEnumerable<Grade>> ShowGrades(CheckGradesVm showGradesVm)
        {
            try
            {
                if (showGradesVm == null)
                    throw new ArgumentNullException ($"View model parameter is null");
                var student = await DbContext.Users.FirstOrDefaultAsync(s => s.Id == showGradesVm.StudentId);

                if (student is null)
                    throw new ArgumentNullException("Can't find student ID");
                
                var StudentOrParent = await DbContext.Users.FirstOrDefaultAsync(s => s.Id == showGradesVm.CurrentUserId);

                if (StudentOrParent is null)
                    throw new ArgumentNullException("Can't find student or parent ID");

                if (!await userManager.IsInRoleAsync(StudentOrParent, "Student") || (await userManager.IsInRoleAsync(StudentOrParent, "Parent")))
                    throw new UnauthorizedAccessException("You can't show grades");

                List<Grade> grades = DbContext.Grades.Where(g => g.StudentId == showGradesVm.StudentId).ToList();
                return grades;
            }catch (Exception exception)
            {
                Logger.LogError(exception.Message);
                throw;
            }
        }
    }
}