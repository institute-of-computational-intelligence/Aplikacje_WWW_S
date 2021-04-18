using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Services
{
  public class TeacherService : BaseService, ITeacherService
  {
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;

    public TeacherService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger,
        UserManager<User> userManager, IConfiguration configuration) : base(dbContext, mapper, logger)
    {
      this._userManager = userManager;
      this._configuration = configuration;
    }

    public async void AddGradeToStudentAsync(AddGradeToStudentVm addGradeToStudentVm)
    {
      try
      {
        if (addGradeToStudentVm is null)
          throw new ArgumentNullException("addGradeToStudentVm");

        var teacher = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == addGradeToStudentVm.TeacherId);

        if (teacher is null)
          throw new ArgumentNullException($"Teacher with id: {addGradeToStudentVm.TeacherId} does not exist");

        if (!await _userManager.IsInRoleAsync(teacher, "Teacher"))
          throw new UnauthorizedAccessException(
              $"User with id {addGradeToStudentVm.TeacherId} does not have required permissions to add grade to the student");

        var student = await DbContext.Users
            .OfType<Student>()
            .FirstOrDefaultAsync(u => u.Id == addGradeToStudentVm.StudentId);

        if (student is null)
          throw new ArgumentNullException($"Student with id: {addGradeToStudentVm.StudentId} does not exist");

        var grade = new Grade
        {
          StudentId = addGradeToStudentVm.StudentId,
          SubjectId = addGradeToStudentVm.SubjectId,
          GradeValue = addGradeToStudentVm.Grade,
          DateOfIssue = DateTime.Now
        };

        await DbContext.Grades.AddAsync(grade);
        await DbContext.SaveChangesAsync();
      }
      catch (Exception ex)
      {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }

    public async void SendEmailAsync(SendEmailVm sendEmailVm)
    {
      try
      {
        var teacher = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == sendEmailVm.TeacherId);

        if (teacher is null)
          throw new ArgumentNullException($"Teacher with id: {sendEmailVm.TeacherId} does not exist");

        var parent = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == sendEmailVm.ParentId);

        if (parent is null)
          throw new ArgumentNullException($"Parent with id: {sendEmailVm.ParentId} does not exist");

        if (!(await _userManager.IsInRoleAsync(teacher, "Teacher") &&
              await _userManager.IsInRoleAsync(parent, "Parent")))
          throw new UnauthorizedAccessException(
              "Insufficient permissions, teacher or parent does not have required permissions");


        var emailUsername = _configuration.GetValue<string>("EmailSettings:Username");
        var emailPassword = _configuration.GetValue<string>("EmailSettings:Password");
        var emailClientAddress = _configuration.GetValue<string>("EmailSettings:ClientAddress");
        var emailClientPort = _configuration.GetValue<int>("EmailSettings:ClientPort");
        var emailClientTimeout = _configuration.GetValue<int>("EmailSettings:ClientTimeout");

        var accessCredentials = new NetworkCredential(emailUsername, emailPassword);
        var message = new MailMessage(teacher.Email, parent.Email, sendEmailVm.EmailSubject,
            sendEmailVm.EmailContent);

        var client = new SmtpClient(emailClientAddress, emailClientPort);

        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.EnableSsl = true;
        client.Timeout = emailClientTimeout;
        client.Credentials = accessCredentials;

        client.Send(message);
      }
      catch (Exception ex)
      {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }
  }
}