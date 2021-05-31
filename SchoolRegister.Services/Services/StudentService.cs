using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace SchoolRegister.Services.Services
{
    public class StudentService : BaseService, IStudentService
    {
        public StudentService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager) : base(dbContext, mapper, logger, userManager)
        {
        }

        public void RemoveStudentFromGroup(StudentVm studentVm)
        {
            try
            {
                if (studentVm is null || studentVm.Id == 0)
                    throw new ArgumentNullException("View model parametr is missing");

                var student = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == studentVm.Id);
                var group = DbContext.Groups.FirstOrDefault(g => g.Name == studentVm.GroupName);

                if (student is null)
                    throw new ArgumentNullException("Student with specified ID doesn't exist.");

                if (group is null)
                    throw new ArgumentNullException("Group with specified name doesn't exist.");

                if (!group.Students.Any(s => s.Id == student.Id))
                    throw new InvalidOperationException("Student does not exist in specified group.");

                student.GroupId = null;
                DbContext.Users.Update(student);

                group.Students.Remove(student);
                DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                throw;
            }
        }

        public void AddStudentToGroup(StudentVm studentVm)
        {
            try
            {
                if (studentVm is null || studentVm.Id == 0)
                    throw new ArgumentNullException("View model parametr is missing");

                var student = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == studentVm.Id);
                var group = DbContext.Groups.FirstOrDefault(g => g.Name == studentVm.GroupName);

                if (student is null)
                    throw new ArgumentNullException("Student with specified ID doesn't exist.");

                if (group is null)
                    throw new ArgumentNullException("Group with specified name doesn't exist.");

                if (group.Students.Any(s => s.Id == student.Id))
                    throw new InvalidOperationException("Student already exist in specified group.");

                student.GroupId = group.Id;
                DbContext.Users.Update(student);

                group.Students.Add(student);
                DbContext.Groups.Update(group);

                DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                throw;
            }
        }

        public StudentVm GetStudent(Expression<Func<Student, bool>> filterExpression)
        {
            try
            {
                if (filterExpression == null)
                    throw new ArgumentNullException($" FilterExpression is null");

                var studentEntity = DbContext.Users.OfType<Student>().FirstOrDefault(filterExpression);
                var studentVm = Mapper.Map<StudentVm>(studentEntity);

                return studentVm;
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                throw;
            }
        }

        public IEnumerable<StudentVm> GetStudents(Expression<Func<Student, bool>> filterExpression = null)
        {
            try
            {
                var studentEntities = DbContext.Users.OfType<Student>().AsQueryable();
                if (filterExpression != null)
                    studentEntities = studentEntities.Where(filterExpression);

                var studentVms = Mapper.Map<IEnumerable<StudentVm>>(studentEntities);

                return studentVms;
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                throw;
            }
        }



    }
}