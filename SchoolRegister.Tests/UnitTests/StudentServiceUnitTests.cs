using System;
using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using SchoolRegister.Tests;
using Xunit;


namespace SchoolRegister.Tests.UnitTests
{
    public class StudentServiceUnitTests : BaseUnitTests
    {
        private readonly IStudentService _studentService;
        public StudentServiceUnitTests(ApplicationDbContext dbContext, IStudentService studentService) : base(dbContext)
        {
            _studentService = studentService;
        }

        [Fact]
        public void AddStudentToGroup()
        {
            var countBefore = DbContext.Groups.First(x => x.Id == 1).Students.Count();
            var addToGroup = new StudentVm()
            {
                StudentId = 10,
                GroupId = 1
            };
            _studentService.AddStudentToGroup(addToGroup);
            
            var countAfter =  DbContext.Groups.First(x => x.Id == 1).Students.Count();

           
            Assert.Contains(10, DbContext.Groups.First(x => x.Id == 1).Students.Select(x => x.Id));
            Assert.Equal(1, DbContext.Users.OfType<Student>().First(x => x.Id == 10).GroupId);
            Assert.Equal(countBefore, countAfter-1);
        }
        [Fact]
        public void RemoveStudentFromGroup()
        {
            var countBefore = DbContext.Groups.First(x => x.Id == 1).Students.Count();
            var removeStudent = new StudentVm()
            {
                StudentId = 6,
                GroupId = 1
            };
            _studentService.RemoveStudentFromGroup(removeStudent);
            
            var countAfter =  DbContext.Groups.First(x => x.Id == 1).Students.Count();

            Assert.Null(DbContext.Users.OfType<Student>().First(x => x.Id == 6).GroupId);     
            Assert.DoesNotContain(6, DbContext.Groups.First(x => x.Id == 1).Students.Select(x => x.Id));
            Assert.Equal(countBefore, countAfter+1);
        }

        
 
    }
}