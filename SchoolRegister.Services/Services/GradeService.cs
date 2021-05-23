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

        public GradesRaportVm ShowGrades(GradesVm gradesVm)
        {
            try{
                if(gradesVm == null)
                    throw new ArgumentNullException($"View model parameter is null!");
                var StudentParent = DbContext.Users.FirstOrDefault(ps => ps.Id == gradesVm.CallerId);
                var Student = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == gradesVm.StudentId);

            

                if(userType.IsInRoleAsync(StudentParent,"Student").Result || userType.IsInRoleAsync(StudentParent,"Parent").Result)
                {
                    if(userType.IsInRoleAsync(StudentParent,"Parent").Result)
                    {
                        if(StudentParent.Id != Student.ParentId)
                            throw new ArgumentException("You can only check Your child's grades!");
                    }else{
                        if(StudentParent.Id != Student.Id)
                            throw new ArgumentException("You can only check Your grades!");
                    }
                    

                    // if(gradesVm.SubjectId != 0)
                    // {
                        
                    // }else{
                    //     gradesEntity = DbContext.Grades.Where(g => (g.StudentId == gradesVm.StudentId && g.SubjectId == gradesVm.SubjectId)).ToList();
                    // }
                } else{
                    throw new UnauthorizedAccessException("You need to have role named 'Parent' or 'Student' to view Grades!");
                }
                var gradesEntity = Mapper.Map<GradesRaportVm>(Student);
                return gradesEntity;

            }catch (Exception ex) {
                Logger.LogError (ex, ex.Message);
                throw;
            }
        }
    }
}