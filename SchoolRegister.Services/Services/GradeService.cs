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
        private readonly UserManager<User> userManager;

        public GradeService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger)
        {
            this.userManager = userManager;
        }
        public async Task<IEnumerable<Grade>> ShowGrades(ShowGradesVm showGradesVm)
        {
            try
            {
                if (showGradesVm == null)
                    throw new ArgumentNullException ($"View model parameter is null");
                var student = await DbContext.Users.FirstOrDefaultAsync(s => s.Id == showGradesVm.StudentId);

                if (student is null)
                    throw new ArgumentNullException("StudentId is incorrect");
                
                var StudentOrParent = await DbContext.Users.FirstOrDefaultAsync(s => s.Id == showGradesVm.StudentOrParentId);

                if (StudentOrParent is null)
                    throw new ArgumentNullException("Id is incorrect");

                if (!(await userManager.IsInRoleAsync(StudentOrParent, "Parent") || await userManager.IsInRoleAsync(StudentOrParent, "Student")))
                    throw new UnauthorizedAccessException("Only student or parent can show grades");

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