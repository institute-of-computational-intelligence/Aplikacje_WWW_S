using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.Model.DataModels;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Services
{
    public class StudentService : BaseService, IStudentService
    {
        public StudentService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger)
        {
        }

        public async Task<GroupVm> AddToGroupAsync(AddToGroupVm addToGroupVm)
        {
            try
            {
                Student student = await DbContext.Users
                    .OfType<Student>()
                    .FirstOrDefaultAsync(u => u.Id == addToGroupVm.StudentId);

                if (student is null)
                    throw new ArgumentNullException($"Nie znaleziono studenta: {addToGroupVm.StudentId}");

                Group group = await DbContext.Groups.FirstOrDefaultAsync(g => g.Id == addToGroupVm.GroupId);

                if (group is null)
                    throw new ArgumentNullException($"Nie znaleziono grupy: {addToGroupVm.GroupId}");


                if (group.Students.Any(x => x.Id == student.Id))
                    throw new DuplicateNameException($"Student ${addToGroupVm.StudentId} już jest zapisany do grupy {addToGroupVm.GroupId}");

                student.GroupId = addToGroupVm.GroupId;
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

        public async Task<GroupVm> RemoveFromGroupAsync(RemoveFromGroupVm removeStudentFromGroup)
        {
            try
            {
                Student student = await DbContext.Users.OfType<Student>().FirstOrDefaultAsync(u => u.Id == removeStudentFromGroup.StudentId);

                if (student is null)
                    throw new ArgumentNullException($"Student with id: {removeStudentFromGroup.StudentId} does not exist");

                Group group = student.Group;

                if (group is null)
                    throw new ArgumentNullException($"Group with id: {removeStudentFromGroup.StudentId} does not exist");

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

        public async Task<Student> ShowStudentAsync(Expression<Func<Student, bool>> filterExpressions)
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

        public IEnumerable<StudentVm> ShowStudents(Expression<Func<Student, bool>> filterExpressions = null)
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