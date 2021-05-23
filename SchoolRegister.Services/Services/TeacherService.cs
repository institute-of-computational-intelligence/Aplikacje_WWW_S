using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Linq.Expressions;



namespace SchoolRegister.Services.Services
{
    public class TeacherService : BaseService, ITeacherService
    {   
        private readonly UserManager<User> userType;
        public TeacherService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger) { userType = userManager; }

        public async Task<Grade> AddGradeToStudent(AddGradeToStudentVm addGradeToStudentVm)
        {
            Grade grade = null;
            try
            {   
                if(addGradeToStudentVm == null)
                    throw new ArgumentNullException ($"View model parameter is null");
                
                var teacher = DbContext.Users.FirstOrDefault(t => t.Id ==  addGradeToStudentVm.TeacherId);
                var subject = DbContext.Subjects.FirstOrDefault(sub => sub.Id == addGradeToStudentVm.SubjectId);
                if(!addGradeToStudentVm.TeacherId.HasValue)
                    throw new ArgumentNullException("TeacherId have no values!");
                if(await userType.IsInRoleAsync(teacher,"Teacher"))
                {   
                    if(teacher.Id != subject.TeacherId)
                        throw new ArgumentException("You can only add grades to subject that You Are teaching!");

                    var student = DbContext.Users.OfType<Student>().FirstOrDefault(t => t.Id == addGradeToStudentVm.StudentId);
                    grade = new Grade()
                        { DateOfIssue = DateTime.Now, GradeValue = addGradeToStudentVm.Grade, StudentId = addGradeToStudentVm.StudentId, SubjectId = addGradeToStudentVm.StudentId };
                    await DbContext.Grades.AddAsync(grade);
                    await DbContext.SaveChangesAsync();
                    return grade;
                }
                else
                {
                    throw new UnauthorizedAccessException("You Don't have permission to add grades!");
                }

            }catch (Exception ex) {
                Logger.LogError (ex, ex.Message);
                throw;
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

                
        public IEnumerable<GroupVm> GetTeacherGroups(TeacherVm getTeachersGroups)
        {
            if (getTeachersGroups == null)
            {
                throw new ArgumentNullException($"Vm is null");
            }
            var teacher = userType.Users.OfType<Teacher>().FirstOrDefault(x => x.Id == getTeachersGroups.Id);
            var teacherGroups = teacher?.Subjects.SelectMany(s=>s.SubjectGroups.Select(gr=>gr.Group));
            var teacherGroupsVm = Mapper.Map<IEnumerable<GroupVm>>(teacherGroups); 
            return teacherGroupsVm;
        }

        public async void SendMailToStudentParent(SendMailToStudentParentVm sendMailToStudentParent)
        {
            try
            {
                if(sendMailToStudentParent == null)
                    throw new ArgumentNullException($"View model paramaeter is null");
                var teacher = DbContext.Users.FirstOrDefault(t => t.Id == sendMailToStudentParent.TeacherId);
                var parent = DbContext.Users.FirstOrDefault(p => p.Id == sendMailToStudentParent.ParentId);
                if(await userType.IsInRoleAsync(teacher,"Teacher") && await userType.IsInRoleAsync(parent,"Parent"))
                {
                    if((sendMailToStudentParent.TeacherId.HasValue) || sendMailToStudentParent.ParentId.HasValue)
                    {
                        MailMessage message = new MailMessage(teacher.Email,parent.Email,sendMailToStudentParent.MailTitle,sendMailToStudentParent.MailContent);
                        
                        SmtpClient client = new SmtpClient()
                        {
                            EnableSsl = true, 
                            Credentials = CredentialCache.DefaultNetworkCredentials
                        };
                        await client.SendMailAsync(message);
                        client.Dispose();
                    }
                    else
                    {
                        throw new ArgumentNullException("TeacherId or ParentId have no values!");
                    }
                }
                else
                {
                    throw new UnauthorizedAccessException("You have valid role to sending emails, or You are not sending email to vaild user type!");
                }

            }catch (Exception ex) {
                Logger.LogError (ex, ex.Message);
                throw;
            }
        }
    }
}