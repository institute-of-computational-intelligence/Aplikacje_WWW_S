using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> userManager;

        public StudentService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger)
            : base(dbContext, mapper, logger)
        {

        }

        public async Task<GroupVm> AddStudentAsync (AttachDetachStudentToGroupVm addToGroupVm)
        {
            try 
            {
                if (addToGroupVm == null)
                    throw new ArgumentNullException ($"View model parameter is null");

                var group = await DbContext.Groups.FirstOrDefaultAsync(g => g.Id == addToGroupVm.GroupId);

                if(group == null)
                    throw new ArgumentNullException("Can't find group ID");

                var student = await DbContext.Users.OfType<Student>().FirstOrDefaultAsync(s => s.Id == addToGroupVm.StudentId);

                if (student == null)
                    throw new ArgumentNullException("Can't find student ID");
                
                var groupVm = Mapper.Map<GroupVm>(group);
                student.GroupId = addToGroupVm.GroupId;
                group.Students.Add(student);
                DbContext.Users.Update(student);
                DbContext.Groups.Update(group);
                await DbContext.SaveChangesAsync();
                return groupVm;

            } catch (Exception ex) 
            {
                Logger.LogError (ex, ex.Message);
                throw;
            }
        }

        public async Task<bool> RemoveStudentAsync(int studentId)
        {
            try
            {
                var studentEntity = await DbContext.Users.OfType<Student>().FirstOrDefaultAsync(x => x.Id == studentId);

                if (!(studentEntity is null))
                {
                    DbContext.Remove(studentEntity);
                    await DbContext.SaveChangesAsync();
                    return true;
                }
                throw new ArgumentNullException($"Student with Id: {studentId} does not exist");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GroupVm> RemoveStudentAsync(RemoveFromGroupVm removeFromGroupVm)
        {
            try
            {
                var student = await DbContext.Users.OfType<Student>().FirstOrDefaultAsync(u => u.Id == removeFromGroupVm.StudentId);

                if (student == null)
                {
                    throw new ArgumentNullException($"Could not find user with id: {removeFromGroupVm.StudentId}");
                }

                student.GroupId = null;

                Group group = await DbContext.Groups.FirstOrDefaultAsync(g => g.Id == removeFromGroupVm.GroupId);

                if (group is null)
                    throw new ArgumentNullException("Can't find group ID");

                var groupVm = Mapper.Map<GroupVm>(group);


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
        public async Task<Student> GetStudentAsync(Expression<Func<Student, bool>> filterExpressions)
        {
            try
            {
                if (filterExpressions is null)
                    throw new ArgumentNullException("Filter expression is null");

                var studentEntity = await DbContext.Users.OfType<Student>().FirstOrDefaultAsync(filterExpressions);

                return studentEntity;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public IEnumerable<StudentVm> GetStudents(Expression<Func<Student, bool>> filterExpressions = null)
        {
            try
            {
                var studentEntities = DbContext.Users.OfType<Student>().AsQueryable();

                if (filterExpressions != null)
                    studentEntities = studentEntities.Where(filterExpressions);

                var studentVms = Mapper.Map<IEnumerable<StudentVm>>(studentEntities);
                return studentVms;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
} 