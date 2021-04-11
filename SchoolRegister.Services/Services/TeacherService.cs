using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.InterFaces;
using SchoolRegister.ViewModels.VM;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
namespace SchoolRegister.Services.Services
{
    public class TeacherService : BaseService, ITeacherService
    {
        private readonly UserManager<User> userManager;
        public TeacherService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger)
        {
            this.userManager = userManager;
        }
       public async void AddGrade(AddGradeVm addGradeVm)
       {
           try{
               if (addGradeVm == null)
                    throw new ArgumentNullException ($"View model parameter is null");

                User teacher = DbContext.Users.OfType<Teacher>().FirstOrDefault( t => t.Id == addGradeVm.teacherId);

                if(!await userManager.IsInRoleAsync(teacher, "teacher"))
                    throw new ArgumentNullException ($"No permission");
                
                Grade grade = new Grade(){
                DateOfissue = addGradeVm.DateOfissue,
                GradeValue = addGradeVm.GradeValue,
                StudentId = addGradeVm.StudentId,
                SubjectId = addGradeVm.SubjectId,
                };

                await DbContext.Grades.AddAsync(grade);
                await DbContext.SaveChangesAsync();
           }catch (Exception ex){
                    Logger.LogError (ex, ex.Message);
                    throw;
                }
       }
    }
}