using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EntityFramework;
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

        public async Task<GroupVm> AddStudentToGroupAsync(AddStudentToGroupVm addStudentToGroup)
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
                    throw new ArgumentNullException($"Group with id: {addStudentToGroup.GroupId} does not exist");


                if (group.Students.Any(x => x.Id == student.Id))
                    throw new DuplicateNameException(
                        $"Group with id: {addStudentToGroup.GroupId} already contains student with id: ${addStudentToGroup.StudentId}");

                student.GroupId = addStudentToGroup.GroupId;
                group.Students.Add(student);

                DbContext.Groups.Update(group);
                DbContext.Users.Update(student);
                await DbContext.SaveChangesAsync();

                var groupVm = Mapper.Map<GroupVm>(group);
                return groupVm;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GroupVm> RemoveStudentFromGroupAsync(RemoveStudentFromGroupVm removeStudentFromGroup)
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

                var groupVm = Mapper.Map<GroupVm>(group);

                if (!group.Students.Remove(student))
                    return null;

                student.GroupId = null;
                DbContext.Groups.Update(group);
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

        public async Task<bool> RemoveStudentAsync(int studentId)
        {
            try
            {
                var studentEntity = await DbContext.Users.OfType<Student>().FirstOrDefaultAsync(x => x.Id == studentId);

                if (studentEntity is null)
                    throw new ArgumentNullException($"Student with Id: {studentId} does not exist");
                DbContext.Remove(studentEntity);
                await DbContext.SaveChangesAsync();
                return true;
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
    }
}