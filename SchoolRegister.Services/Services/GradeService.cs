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
        private readonly UserManager<User> userType;
        public GradeService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger) { userType = userManager; }

        public async Task<IEnumerable<Grade>> ShowGrades(GradesVm gradesVm)
        {
            try{
                if(gradesVm == null)
                    throw new ArgumentNullException($"View model parameter is null");
                var StudentParent = DbContext.Users.FirstOrDefault(ps => ps.Id == gradesVm.CallerId);
                List<Grade> gradesEntity;
                if(await userType.IsInRoleAsync(StudentParent,"Student") || await userType.IsInRoleAsync(StudentParent,"Parent"))
                {
                    
                    if(gradesVm.SubjectId != 0)
                    {
                        gradesEntity = DbContext.Grades.Where(g => g.StudentId == gradesVm.StudentId).ToList();
                    }else{
                        gradesEntity = DbContext.Grades.Where(g => (g.StudentId == gradesVm.StudentId && g.SubjectId == gradesVm.SubjectId)).ToList();
                    }
                } else{
                    throw new ArgumentNullException("You neet to have role named 'Parent' or 'Student' to view Grades");
                }
                return gradesEntity;

            }catch (Exception ex) {
                Logger.LogError (ex, ex.Message);
                throw;
            }
        }
    }
}