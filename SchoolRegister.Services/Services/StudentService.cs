using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;


namespace SchoolRegister.Services.Services
{
    
    public class StudentService : BaseService, IStudentService
    {
        public StudentService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger) { }

        public void AddOrRemoveStudentGroup(StudentVm studentVm)
        {
            try{
                if(studentVm == null)
                    throw new ArgumentNullException($"View model parameter is null");
                var student = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == studentVm.Id);
                var group = DbContext.Groups.FirstOrDefault(g => g.Name == studentVm.GroupName);

                if(student == null)
                    throw new ArgumentNullException("Specifed student doesn't exist");
                if(group == null)
                    throw new ArgumentNullException("Specifed group doesn't exist");
                if(!group.Students.Any(s => s.Id == student.Id))
                {   
                    student.GroupId = group.Id;
                    DbContext.Update(student);
                    group.Students.Add(student);
                }else{
                    student.GroupId = null;
                    DbContext.Update(student);
                    group.Students.Remove(student);
                    }
                DbContext.SaveChanges();
                
            }catch (Exception ex) {
                Logger.LogError (ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<StudentVm> GetStudents (Expression<Func<Student, bool>> filterPredicate = null) {
            var studentsEntities = DbContext.Users.OfType<Student> ().AsQueryable ();
            if (filterPredicate != null)
                studentsEntities = studentsEntities.Where (filterPredicate);
            var studentsVm = Mapper.Map<IEnumerable<StudentVm>> (studentsEntities);
            return studentsVm;
        }

        public StudentVm GetStudent (Expression<Func<Student, bool>> filterPredicate) {
            if (filterPredicate == null) throw new ArgumentNullException ($"filterPredicate is null");
            var studentEntity = DbContext.Users.OfType<Student> ().FirstOrDefault (filterPredicate);
            var studentVm = Mapper.Map<StudentVm> (studentEntity);
            return studentVm;
        }
    }
}