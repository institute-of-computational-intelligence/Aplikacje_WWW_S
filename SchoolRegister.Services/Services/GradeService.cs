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

        public async Task<IEnumerable<DisplayGradeVm>> DisplayGrades(GetGradesVm gradesVm)
        {
            try
            {
                if (gradesVm is null)
                    throw new ArgumentNullException("View model parametr is missing");

                var parentOrStudent = DbContext.Users.FirstOrDefault(ps => ps.Id == gradesVm.CallerId);
                var student = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == gradesVm.StudentId);

                if (parentOrStudent is null || student is null)
                    throw new ArgumentNullException("Could not find specified Student or Parent");

                if (!(await UserManager.IsInRoleAsync(parentOrStudent, "Parent") || await UserManager.IsInRoleAsync(parentOrStudent, "Student")))
                    throw new UnauthorizedAccessException("Access denied. Only Student or Parent have privileges to call this method");

                List<Grade> grades;

                if (parentOrStudent.Id == student.Id)
                    grades = student.Grades.ToList();
                else if (parentOrStudent.Id == student.ParentId)
                    grades = student.Grades.ToList();
                else
                    throw new UnauthorizedAccessException("Access denied. Only your parent or student himself have permissions to view grades");

                var displayGradesVm = Mapper.Map<List<DisplayGradeVm>>(grades);
                return displayGradesVm;
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}