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
  public class StudentService : BaseService, IStudentService
  {
    public StudentService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger)
    {
    }


    public async Task<GroupVm> AddStudentToGroupVm(AddStudentToGroupVm addStudentToGroupVm)
    {
      try
      {
        if (addStudentToGroupVm == null)
          throw new ArgumentNullException($"View model parameter is null");

        var group = await DbContext.Groups.FirstOrDefaultAsync(g => g.Id == addStudentToGroupVm.GroupId);

        if (group == null)
          throw new ArgumentNullException("Can't find group ID");

        var student = await DbContext.Users.OfType<Student>().FirstOrDefaultAsync(s => s.Id == addStudentToGroupVm.StudentId);

        if (student == null)
          throw new ArgumentNullException("Can't find student ID");

        var groupVm = Mapper.Map<GroupVm>(group);
        student.GroupId = addStudentToGroupVm.GroupId;
        group.Students.Add(student);
        DbContext.Users.Update(student);
        DbContext.Groups.Update(group);
        await DbContext.SaveChangesAsync();
        return groupVm;

      }
      catch (Exception ex)
      {
        Logger.LogError(ex, ex.Message);
        throw;
      }
    }

    public async Task<GroupVm> RemoveStudentFromGroupVm(RemoveStudentFromGroupVm removeStudentFromGroupVm)
    {
      try
      {
        if (removeStudentFromGroupVm == null)
          throw new ArgumentNullException($"View model parameter is null");
        var student = await DbContext.Users.OfType<Student>().FirstOrDefaultAsync(s => s.Id == removeStudentFromGroupVm.StudentId);

        if (student == null)
          throw new ArgumentNullException("Can't find student ID");

        Group group = await DbContext.Groups.FirstOrDefaultAsync(g => g.Id == removeStudentFromGroupVm.GroupId);


        if (group is null)
          throw new ArgumentNullException("Can't find group ID");


        var groupVm = Mapper.Map<GroupVm>(group);
        student.GroupId = null;
        DbContext.Users.Update(student);
        await DbContext.SaveChangesAsync();
        return groupVm;
      }
      catch (Exception ex)
      {

        Logger.LogError(ex, ex.Message);
        throw;
      }
    }

  }
}