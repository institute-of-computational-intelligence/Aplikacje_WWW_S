using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Services
{
    public class StudentService : BaseService, IStudentService
    {
        public StudentService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger){
        }

        public async void AddStudentAsync(AddStudentVm AddStudentVm)
        {
            try 
            {
                if (AddStudentVm == null)
                    throw new ArgumentNullException ($"View model parameter is null");

                var group = await DbContext.Groups.FirstOrDefaultAsync(g => g.Id == AddStudentVm.GroupId);

                if(group == null)
                    throw new ArgumentNullException("Can't find group ID");

                var student = await DbContext.Users.OfType<Student>().FirstOrDefaultAsync(s => s.Id == AddStudentVm.StudentId);

                if (student == null)
                    throw new ArgumentNullException("Can't find student ID");
                

                student.GroupId = AddStudentVm.GroupId;
                group.Students.Add(student);
                DbContext.Users.Update(student);
                DbContext.Groups.Update(group);
                await DbContext.SaveChangesAsync();

            } catch (Exception ex) 
            {
                Logger.LogError (ex, ex.Message);
                throw;
            }
        }

        public async void RemoveStudentAsync(RemoveStudentVm RemoveStudentVm)
        {
            try 
            {
                if (RemoveStudentVm == null)
                    throw new ArgumentNullException ($"View model parameter is null");
                var student = await DbContext.Users.OfType<Student>().FirstOrDefaultAsync(s => s.Id == RemoveStudentVm.StudentId);

                if (student == null)
                    throw new ArgumentNullException("Can't find student ID");
                
                student.GroupId = null;
                DbContext.Users.Update(student);
                await DbContext.SaveChangesAsync();
            } catch (Exception ex) 
            {
                Logger.LogError (ex, ex.Message);
                throw;
            }
        }
    }
}