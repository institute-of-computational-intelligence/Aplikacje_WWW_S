using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace SchoolRegister.Services.Services
{
    public class GradeService : BaseService, IGradeService
    {
        private readonly UserManager<User> userManager;
        public GradeService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger)
        {
            this.userManager = userManager;
        }

   
        public async Task<IEnumerable<Grade>> GetGrades(GetGradesVm getGradesVm)
        {
            try{
                var user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == getGradesVm.UserId);
                if(getGradesVm == null)
                    throw new ArgumentNullException ($"View model parameter is null");
                if(await userManager.IsInRoleAsync(user, "Parent") || await userManager.IsInRoleAsync(user, "Student"))
                {
                    var grades = DbContext.Grades.Where(g => g.StudentId == getGradesVm.StudentId).ToList();
                    return grades;
                }



                   
                else
                    throw new ArgumentNullException ($"No permissions");
            } catch (Exception ex) {
                Logger.LogError (ex, ex.Message);  
                throw;
            }
        }

        public GradeVm AddGrade(AddGradeVm addGradeVm)
        {
            if (addGradeVm == null)
            {
                throw new ArgumentNullException($"GradingStudent Vm is null");
            }
            var user = userManager.FindByIdAsync(addGradeVm.TeacherId.ToString()).Result;
            if (user == null || !userManager.IsInRoleAsync(user, "Teacher").Result || user is not Teacher)
            {
                throw new InvalidOperationException("The you are not Teacher, you cannot add grades.");
            }
            var student = DbContext.Users.OfType<Student>().FirstOrDefault(t => t.Id == addGradeVm.StudentId);
            if (student == null)
            {
                throw new ArgumentNullException($"Student is null");
            }
            var subject = DbContext.Subjects.FirstOrDefault(s => s.Id == addGradeVm.SubjectId);
            if (subject == null)
            {
                throw new ArgumentNullException($"Subject is null");
            }
            if (subject.TeacherId != addGradeVm.TeacherId)
                throw new ArgumentException("The teacher cannot add grade for this subject");


            
            var gradeEntity = Mapper.Map<Grade>(addGradeVm);
            DbContext.Grades.Add(gradeEntity);
            DbContext.SaveChanges();
            var gradeVm = Mapper.Map<GradeVm>(gradeEntity);
            return gradeVm;
        }
           
 public async Task<GradeVm> AddGradeAsync(AddGradeVm addGradeAsync)
        {             
            try
            {
                if (addGradeAsync == null)
                    throw new ArgumentNullException("Grade can not be null");

                var teacherEntity = await DbContext.Users.FirstOrDefaultAsync(t => t.Id == addGradeAsync.TeacherId);

                var studentEntity = await DbContext.Users.FirstOrDefaultAsync(s => s.Id == addGradeAsync.StudentId);

                if (teacherEntity == null || studentEntity == null)
                    throw new ArgumentException($"There is no user with id {addGradeAsync.StudentId} or {addGradeAsync.TeacherId}.");

                if (!await userManager.IsInRoleAsync(teacherEntity, "Teacher"))
                    throw new UnauthorizedAccessException("Only teacher are allowed to add a grade to student.");

                var grade = Mapper.Map<Grade>(addGradeAsync);
                await DbContext.Grades.AddAsync(grade);
                await DbContext.SaveChangesAsync();

                var gradeVm = Mapper.Map<GradeVm>(grade);
                return gradeVm;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
                
            }
        }
