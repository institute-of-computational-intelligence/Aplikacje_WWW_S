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
    public class GradeService : BaseService, IGradeService
    {
        public GradeService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger, userManager)
        {
        }


        public async Task<IEnumerable<Grade>> GetGrades(GradeVm gradesVm)
        {
            try
            {
                if(gradesVm is null)
                    throw new ArgumentNullException($"View model parametr is missing");

                var user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == gradesVm.UserId);

                List <Grade> grades;
                if (await UserManager.IsInRoleAsync(user, "Parent") || await UserManager.IsInRoleAsync(user, "Student"))
                {
                    grades = DbContext.Grades.Where(g => g.StudentId == gradesVm.StudentId).ToList();
                    return grades;
                }
                else
                {
                    throw new UnauthorizedAccessException("Current user don't have permision to check grades.");
                }
            }
            catch (Exception exception)
            {
                Logger.LogError(exception.Message);
                throw;
            }
        }
    }
}