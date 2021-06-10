using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System.Threading.Tasks;


namespace SchoolRegister.Services.Services
{
    public class TeacherService : BaseService, ITeacherService
    {
        private readonly UserManager<User> userManager;

        public TeacherService(UserManager<User> userManager, ApplicationDbContext dbContext, IMapper mapper, ILogger logger) 
            : base(dbContext, mapper, logger)
        {
            this.userManager = userManager;
        }
        public async Task<Grade> AddGrade(AddGradeVm addGradeVm)
        {
            try {

                if (addGradeVm == null)
                    throw new ArgumentNullException ($"View model parameter is null");       

                var teacher = DbContext.Users.OfType<Teacher>().FirstOrDefault(t => t.Id == addGradeVm.TeacherId);
                if(teacher is null) 
                    throw new ArgumentNullException("Can't find teacher ID");      

                if(await userManager.IsInRoleAsync(teacher, "Teacher"))
     
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
        public async void SendEmail(SendEmailVm sendEmailVm)
        {
            try{
            var sender = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == sendEmailVm.SenderId);
            
            if(sender == null)
            {
                throw new ArgumentNullException("Could not find specified SenderId.");
            }
            
            var recipient = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == sendEmailVm.RecipientId);

            if(recipient == null)
            {
                throw new ArgumentNullException("Could not find specified SenderId.");
            }

            if (await userManager.IsInRoleAsync(sender, "Teacher") && await userManager.IsInRoleAsync(recipient, "Parent"))
            {
                var message = new MailMessage(sender.Email, recipient.Email, sendEmailVm.EmailTitle, sendEmailVm.MailContent);
                
                string sendEmailsFrom = "emailAddress@gmail.com";             
                string sendEmailsFromPassword = "strongPassword";
                NetworkCredential credentials = new NetworkCredential(sendEmailsFrom, sendEmailsFromPassword);

                var client = new SmtpClient("smtp.gmail.com", 587);
                
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Timeout = 20000;
                client.Credentials = credentials;
                
                client.Send(message); 
            }
            }catch(Exception ex){
                Logger.LogError(ex, ex.Message);
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

        public IEnumerable<GroupVm> GetTeachersGroups(TeachersGroupsVm getTeachersGroups)
        {
            if (getTeachersGroups == null)
            {
                throw new ArgumentNullException($"Vm is null");
            }
            var teacher = userManager.Users.OfType<Teacher>().FirstOrDefault(x => x.Id == getTeachersGroups.TeacherId);
            var teacherGroups = teacher?.Subjects.SelectMany(s=>s.SubjectGroups.Select(gr=>gr.Group));
            var teacherGroupsVm = Mapper.Map<IEnumerable<GroupVm>>(teacherGroups); 
            return teacherGroupsVm;
        }
    }
}