using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Services
{
    public class GradeService : BaseService, IGradeService
    {
        private readonly UserManager<User> _userManager;
        public GradeService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger)
        {
            _userManager = userManager;
        }

        public GradeVm AddGradeToStudent(AddGradeToStudentVm addGradeToStudentVm)
        {
            if (addGradeToStudentVm == null)
            {
                throw new ArgumentNullException($"GradingStudent Vm is null");
            }
            var user = _userManager.FindByIdAsync(addGradeToStudentVm.TeacherId.ToString()).Result;
            if (user == null || !_userManager.IsInRoleAsync(user, "Teacher").Result || user is not Teacher)
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

        public GradesReportVm GetGradesReportForStudent(GetGradesReportVm getGradesVm)
        {
            if (getGradesVm == null)
            {
                throw new ArgumentNullException($"Get grades Vm is null");
            }
            var getterUser = DbContext.Users.FirstOrDefault(x => x.Id == getGradesVm.GetterUserId);
            if (getterUser == null) throw new ArgumentNullException($"Getter user is null");
            if (!_userManager.IsInRoleAsync(getterUser, "Teacher").Result &&
                !_userManager.IsInRoleAsync(getterUser, "Student").Result &&
                !_userManager.IsInRoleAsync(getterUser, "Parent").Result &&
                !_userManager.IsInRoleAsync(getterUser, "Admin").Result)
            {
                throw new InvalidOperationException("The getter user don't have permissions to read.");
            }
            var student = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == getGradesVm.StudentId);
            if (student == null) throw new InvalidOperationException("the given user is not student");

            if (_userManager.IsInRoleAsync(getterUser, "Teacher").Result && getterUser is Teacher)
            {
                var subject = student.Group.SubjectGroups.Select(s => s.Subject).FirstOrDefault(t => t.TeacherId == getterUser.Id);
                if (subject == null)
                    throw new InvalidOperationException("Teacher not running classes within the student group.");
            }

            if (_userManager.IsInRoleAsync(getterUser, "Student").Result && getterUser.Id != student.Id)
            {
                throw new InvalidOperationException("Other student cannot read other students grades");
            }
            if (_userManager.IsInRoleAsync(getterUser, "Parent").Result && getterUser is Parent)
            {
                if (student.ParentId != getterUser.Id)
                    throw new InvalidOperationException("This given user is not a parent of this student.");
            }

            var gradesReport = Mapper.Map<GradesReportVm>(student);
            return gradesReport;
        }
    }
}