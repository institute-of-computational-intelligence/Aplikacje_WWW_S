using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        public async Task<Grade> AddGradeAsync(AddGradeAsyncVm addGradeVm)
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

        public async void SendEmailToParent(SendEmailVm sendMailVm)
        {
            try
            {
                if (sendMailVm == null)
                    throw new ArgumentNullException ($"View model parameter is null");
                var sender = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == sendMailVm.SenderId);

                if (sender is null)
                    throw new ArgumentNullException("Can't find sender ID");

                var recipient = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == sendMailVm.RecipientId);

                if (recipient is null)
                    throw new ArgumentNullException("Can't find recipient ID");

                if (await userManager.IsInRoleAsync(sender, "Teacher") && await userManager.IsInRoleAsync(recipient, "Parent"))
                {
                    var client = new SmtpClient();
                    var message = new MailMessage(sender.Email, recipient.Email, sendMailVm.EmailSubject, sendMailVm.EmailBody);
                    client.Send(message); 
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}