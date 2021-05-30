using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.Model.DataModels;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
namespace SchoolRegister.Services.Services
{
    public class TeacherService : BaseService, ITeacherService
    {
        private readonly IEmailSenderService _emailService;
        private readonly UserManager<User> _userManager;

        public TeacherService(ApplicationDbContext dbContext, 
                                IMapper mapper, 
                                ILogger logger, 
                                UserManager<User> userManager,
                                IEmailSenderService emailService) 
        : base(dbContext,  mapper, logger)
        {
            _userManager = userManager;
            _emailService = emailService;
        }


          public async Task<Grade> AddGrade(AddGradeVm addGradeVm)
        {
            try {

                if (addGradeVm == null)
                    throw new ArgumentNullException ($"View model parameter is null");       
 
                var teacher = DbContext.Users.OfType<Teacher>().FirstOrDefault(t => t.Id == addGradeVm.TeacherId);
                if(teacher is null) 
                    throw new ArgumentNullException("Can't find teacher ID");      

                if(await _userManager.IsInRoleAsync(teacher, "Teacher"))
     
                {
                               
                    var student = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == addGradeVm.StudentId);
                    if(student is null) 
                        throw new ArgumentNullException("Can't find student ID");
                                
                    Grade grade = new Grade(){
                        DateOfIssue = DateTime.Now,
                        GradeValue = addGradeVm.GradeValue,
                        StudentId = addGradeVm.StudentId,        
                        SubjectId = addGradeVm.SubjectId
                    };
                    await DbContext.Grades.AddAsync(grade);
                    await DbContext.SaveChangesAsync();
                    return grade;
                }
                else
                    throw new ArgumentNullException("You can't add grades");
                
            } catch (Exception ex) {
                Logger.LogError (ex, ex.Message);
                throw;
                   
            }
        }  

        public async Task<bool> SendEmailToParentAsync(SendEmailVm sendEmailVm)
        {
            try
            {
                if (sendEmailVm == null)
                {
                    throw new ArgumentNullException($"Vm is null");
                }

                var teacher = DbContext.Users.OfType<Teacher>()
                    .FirstOrDefault(x => x.Id == sendEmailVm.SenderId);
                if (teacher == null || _userManager.IsInRoleAsync(teacher, "Teacher").Result == false)
                {
                    throw new InvalidOperationException("sender is not teacher");
                }

                var student = DbContext.Users.OfType<Student>().FirstOrDefault(x => x.Id == sendEmailVm.StudentId);
                if (student == null || !_userManager.IsInRoleAsync(student, "Student").Result)
                {
                    throw new InvalidOperationException("given user is not student");
                }
                await _emailService.SendEmailAsync(student.Parent.Email, teacher.Email, sendEmailVm.Title,sendEmailVm.Content);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<TeacherVm> GetTeachers(Expression<Func<Teacher, bool>> filterPredicate = null)
        {
            var teacherEntities = DbContext.Users.OfType<Teacher>()
                                    .AsQueryable();
            if (filterPredicate != null)
            {
                teacherEntities = teacherEntities.Where(filterPredicate);
            }
            var teacherVms = Mapper.Map<IEnumerable<TeacherVm>>(teacherEntities);
            return teacherVms;
        }

        public TeacherVm GetTeacher(Expression<Func<Teacher, bool>> filterPredicate)
        {
            var teacherEntity = DbContext.Users.OfType<Teacher>().FirstOrDefault();
            if (teacherEntity == null)
            {
                throw new InvalidOperationException("There is no such teacher");
            }

            var teacherVm = Mapper.Map<TeacherVm>(teacherEntity);
            return teacherVm;
        }

        public IEnumerable<GroupVm> GetTeachersGroups(TeachersGroupsVm getTeachersGroups)
        {
            if (getTeachersGroups == null)
            {
                throw new ArgumentNullException($"Vm is null");
            }
            var teacher = _userManager.Users.OfType<Teacher>().FirstOrDefault(x => x.Id == getTeachersGroups.TeacherId);
            var teacherGroups = teacher?.Subjects.SelectMany(s=>s.SubjectGroups.Select(gr=>gr.Group));
            var teacherGroupsVm = Mapper.Map<IEnumerable<GroupVm>>(teacherGroups); 
            return teacherGroupsVm;
        }

        public async Task<TeacherVm> GetTeacherAsync(Expression<Func<Teacher, bool>> filterExpressions)
        {
            try
            {
                if (filterExpressions == null)
                    throw new ArgumentNullException("filterExpressions is null");

                var teacherEntity = await DbContext.Users.OfType<Teacher>().FirstOrDefaultAsync(filterExpressions);

                var teacherVm = Mapper.Map<TeacherVm>(teacherEntity);
                return teacherVm;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}