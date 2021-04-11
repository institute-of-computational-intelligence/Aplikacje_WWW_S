using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.InterFaces;
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
        public void AddStudentToGroup(AddOrRmvStudentGroup addStudentToGroup)
        {
            try{
                if(addStudentToGroup == null)
                    throw new ArgumentNullException ($"View model parameter is null");

                Student student = DbContext.Users.OfType<Student>().FirstOrDefault( t => t.Id == addStudentToGroup.StudentId);

                if (student == null)
                    throw new ArgumentNullException ($"Wrong StudentId");

                Group group = DbContext.Groups.FirstOrDefault( t => t.Id == addStudentToGroup.GroupId);

                 if (group == null)
                    throw new ArgumentNullException ($"Wrong GroupId");

               student.GroupId = addStudentToGroup.GroupId;
               group.Students.Add(student);

               DbContext.Groups.Update(group);
               DbContext.Users.Update(student);
               DbContext.SaveChanges();

            }catch (Exception ex) {
                Logger.LogError (ex, ex.Message);
                throw;
            }
        }
        public void RemoveStudentFromGroup(AddOrRmvStudentGroup removeStudentFromGroup)
        {
             try{
                if(removeStudentFromGroup == null)
                    throw new ArgumentNullException ($"View model parameter is null");

                Student student = DbContext.Users.OfType<Student>().FirstOrDefault( t => t.Id == removeStudentFromGroup.StudentId);

                if (student == null)
                    throw new ArgumentNullException ($"Wrong StudentId");

                Group group = DbContext.Groups.FirstOrDefault( t => t.Id == removeStudentFromGroup.GroupId);

                 if (group == null)
                    throw new ArgumentNullException ($"Wrong GroupId");

               
               group.Students.Remove(student);
               student.GroupId = null;
               DbContext.Groups.Update(group);
               DbContext.Users.Update(student);
               DbContext.SaveChanges();

               

            }catch (Exception ex) {
                Logger.LogError (ex, ex.Message);
                throw;
            }
        }
    }
}