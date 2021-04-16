using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRegister.Services.Services
{
    public class GradeService : BaseService, IGradeService
    {
        public GradeService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger, userManager)
        {
        }

        public async Task<IEnumerable<Grade>> DisplayGrades(GetGradesVm gradesVm)
        {
            try
            {
                if (gradesVm is null)
                    throw new ArgumentNullException("View model parametr is missing");

                var parentOrStudent = DbContext.Users.FirstOrDefault(ps => ps.Id == gradesVm.CallerId);
                List<Grade> grades;

                if (await UserManager.IsInRoleAsync(parentOrStudent, "Parent") || await UserManager.IsInRoleAsync(parentOrStudent, "Student"))
                    grades = DbContext.Grades.Where(g => g.StudentId == gradesVm.StudentId).ToList();
                else
                    throw new UnauthorizedAccessException("Access denied. Only Student or Parent have privileges to call this method");

                return grades;
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}