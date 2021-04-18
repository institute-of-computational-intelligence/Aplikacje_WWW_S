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

    public GradeService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger,
        UserManager<User> userManager) : base(dbContext, mapper, logger)
    {
      this.userManager = userManager;
    }

    public async Task<IEnumerable<Grade>> GetGradesAsync(GetGradesVm getGradesVm)
    {
      try
      {
        var user = await DbContext.Users
            .FirstOrDefaultAsync(u => u.Id == getGradesVm.StudentId);

        if (user is null)
          throw new ArgumentNullException($"User with id: {getGradesVm.StudentId} does not exist");

        if (!(await userManager.IsInRoleAsync(user, "Parent") ||
              await userManager.IsInRoleAsync(user, "Student")))
          throw new UnauthorizedAccessException(
              "Insufficient permissions, only student or parent can access the grades");

        var grades = DbContext.Grades.Where(g => g.StudentId == getGradesVm.StudentId).ToList();

        return grades;
      }
      catch (Exception exception)
      {
        Logger.LogError(exception.Message);
        throw;
      }
    }
  }
}