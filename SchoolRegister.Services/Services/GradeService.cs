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
                if (gradesVm == null)
                    throw new ArgumentNullException($"View model parametr is missing");

                var user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == gradesVm.UserId);

                List<Grade> grades;
                if (user is null)
                    throw new ArgumentNullException("User doesn't exist");

                var student = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == gradesVm.StudentId);

                if (await UserManager.IsInRoleAsync(user, "Parent"))
                {

                    if (student.ParentId != gradesVm.UserId)
                        throw new UnauthorizedAccessException("You can only check your own child's grades");

                    grades = DbContext.Grades.Where(g => g.StudentId == gradesVm.StudentId).ToList();
                    return grades;
                }
                else if (await UserManager.IsInRoleAsync(user, "Student"))
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