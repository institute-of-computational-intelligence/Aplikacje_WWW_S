using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
    }
}