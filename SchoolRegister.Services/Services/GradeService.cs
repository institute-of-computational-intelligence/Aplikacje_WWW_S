using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;


namespace SchoolRegister.Services.Services
{
    public class GradeService : BaseService, IGradeService
    {
        private readonly UserManager<User> userManager;

        public GradeService(UserManager<User> userManager, ApplicationDbContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger)
        {
            this.userManager = userManager;
        }

        public async Task<IEnumerable<Grade>> GetGrades(CheckGradesVm checkGradesVm)
        {
            try
            {
                var user = DbContext.Users.FirstOrDefault(u => u.Id == checkGradesVm.CurrentUserId);

                if (user == null)
                {
                    throw new ArgumentNullException($"Could not find user with id: {checkGradesVm.CurrentUserId}");
                }

                if (await userManager.IsInRoleAsync(user, "Parent") || await userManager.IsInRoleAsync(user, "Student"))
                {
                    var grades = DbContext.Grades.Where(g => g.StudentId == checkGradesVm.StudentId).ToList();

                    return grades;
                }
                else
                {
                    throw new ArgumentException("Current user does not have required permissions to performe this action.");
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