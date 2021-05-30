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

        public GradeService(UserManager<User> userManager, ApplicationDbContext dbContext, IMapper mapper, ILogger logger) 
            : base(dbContext, mapper, logger)
        {
            this.userManager = userManager;
        }

        public async Task<IEnumerable<Grade>> GetGrades(GradeVm gradeVm)
        {
            try
            {
                var user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == gradeVm.UserId); 

                if (user == null)
                {
                    throw new ArgumentNullException($"Could not find user with id: {gradeVm.UserId}");
                }

                if(await userManager.IsInRoleAsync(user, "Parent") || await userManager.IsInRoleAsync(user, "Student"))
                {
                    var grades = DbContext.Grades.Where(g => g.StudentId == gradeVm.StudentId).ToList();

                    return grades; 
                }
                else
                {
                    throw new ArgumentException("Current user does not have required permissions to performe this action.");
                }
            }
            catch(Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public GradesReportVm GetGradesReportForStudent(GetGradesReportVm getGradesVm)
        {
            if (getGradesVm == null)
            {
                throw new ArgumentNullException($"Get grades Vm is null");
            }
            var getterUser = DbContext.Users.FirstOrDefault(x => x.Id == getGradesVm.GetterUserId);
            if (getterUser == null) throw new ArgumentNullException($"Getter user is null");
            if (!userManager.IsInRoleAsync(getterUser, "Teacher").Result &&
                !userManager.IsInRoleAsync(getterUser, "Student").Result &&
                !userManager.IsInRoleAsync(getterUser, "Parent").Result &&
                !userManager.IsInRoleAsync(getterUser, "Admin").Result)
            {
                throw new InvalidOperationException("The getter user don't have permissions to read.");
            }
            var student = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == getGradesVm.StudentId);
            if (student == null) throw new InvalidOperationException("the given user is not student");

            if (userManager.IsInRoleAsync(getterUser, "Teacher").Result && getterUser is Teacher)
            {
                var subject = student.Group.SubjectGroups.Select(s => s.Subject).FirstOrDefault(t => t.TeacherId == getterUser.Id);
                if (subject == null)
                    throw new InvalidOperationException("Teacher not running classes within the student group.");
            }

            if (userManager.IsInRoleAsync(getterUser, "Student").Result && getterUser.Id != student.Id)
            {
                throw new InvalidOperationException("Other student cannot read other students grades");
            }
            if (userManager.IsInRoleAsync(getterUser, "Parent").Result && getterUser is Parent)
            {
                if (student.ParentId != getterUser.Id)
                    throw new InvalidOperationException("This given user is not a parent of this student.");
            }

            var gradesReport = Mapper.Map<GradesReportVm>(student);
            return gradesReport;
        }

         public GradeVm AddGradeToStudent(AddGradeVm addGradeToStudentVm)
        {
            if (addGradeToStudentVm == null)
            {
                throw new ArgumentNullException($"GradingStudent Vm is null");
            }
            var user = userManager.FindByIdAsync(addGradeToStudentVm.TeacherId.ToString()).Result;
            if (user == null || !userManager.IsInRoleAsync(user, "Teacher").Result || user is not Teacher)
            {
                throw new InvalidOperationException("The you are not Teacher, you cannot add grades.");
            }
            var student = DbContext.Users.OfType<Student>().FirstOrDefault(t => t.Id == addGradeToStudentVm.StudentId);
            if (student == null)
            {
                throw new ArgumentNullException($"Student is null");
            }
            var subject = DbContext.Subjects.FirstOrDefault(s => s.Id == addGradeToStudentVm.SubjectId);
            if (subject == null)
            {
                throw new ArgumentNullException($"Subject is null");
            }
            if (subject.TeacherId != addGradeToStudentVm.TeacherId)
                throw new ArgumentException("The teacher cannot add grade for this subject");

            var gradeEntity = Mapper.Map<Grade>(addGradeToStudentVm);
            DbContext.Grades.Add(gradeEntity);
            DbContext.SaveChanges();
            var gradeVm = Mapper.Map<GradeVm>(gradeEntity);
            return gradeVm;
        }


    }
}