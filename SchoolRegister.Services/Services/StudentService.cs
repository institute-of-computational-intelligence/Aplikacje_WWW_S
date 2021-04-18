using System;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Services
{
  public class StudentService : BaseService, IStudentService
  {
    public StudentService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper,
        logger)
    {
    }

    public async void AddStudentToGroupAsync(AddStudentToGroupVm addStudentToGroup)
    {
      try
      {
        var student = await DbContext.Users
            .OfType<Student>()
            .FirstOrDefaultAsync(u => u.Id == addStudentToGroup.StudentId);

        if (student is null)
          throw new ArgumentNullException($"Student with id: {addStudentToGroup.StudentId} does not exist");

        var group = await DbContext.Groups.FirstOrDefaultAsync(g => g.Id == addStudentToGroup.GroupId);

        if (group is null)
          throw new ArgumentNullException($"Group with id: {addStudentToGroup.StudentId} does not exist");


        student.GroupId = addStudentToGroup.GroupId;
        group.Students.Add(student);

        DbContext.Groups.Update(group);
        DbContext.Users.Update(student);
        await DbContext.SaveChangesAsync();
      }
      catch (Exception ex)
      {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }

    public async void RemoveStudentFromGroupAsync(RemoveStudentFromGroupVm removeStudentFromGroup)
    {
      try
      {
        var student = await DbContext.Users.OfType<Student>()
            .FirstOrDefaultAsync(u => u.Id == removeStudentFromGroup.StudentId);

        if (student is null)
          throw new ArgumentNullException(
              $"Student with id: {removeStudentFromGroup.StudentId} does not exist");

        var group = await DbContext.Groups.FirstOrDefaultAsync(g => g.Id == removeStudentFromGroup.GroupId);

        if (group is null)
          throw new ArgumentNullException(
              $"Group with id: {removeStudentFromGroup.StudentId} does not exist");

        student.GroupId = null;
        group.Students.Remove(student);

        DbContext.Groups.Update(group);
        DbContext.Users.Update(student);
        await DbContext.SaveChangesAsync();
      }
      catch (Exception ex)
      {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }
  }
}