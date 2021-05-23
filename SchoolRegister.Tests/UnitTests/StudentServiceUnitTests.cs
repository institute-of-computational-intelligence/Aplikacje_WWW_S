using System.Linq;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using Xunit;
using System.Data;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

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
        public async void AddStudentToGroup()
        {
            var addStudentToGroup = new AddToGroupVm()
            {
                StudentId = 6,
                GroupId = 3,
            };

            var group = await _studentService.AddStudentAsync(addStudentToGroup);

            Assert.NotNull(group);
            Assert.Contains(6, DbContext.Groups.First(x => x.Id == 3).Students.Select(x => x.Id));
            Assert.Equal(3, DbContext.Users.OfType<Student>().First(x => x.Id == 6).GroupId);
        }
        
        [Fact]
        public async void RemoveStudentFromGroup()
        {
            var removeStudentFromGroup = new RemoveFromGroupVm()
            {
                StudentId = 6,
                GroupId = 3,
            };

            await _studentService.RemoveStudentAsync(removeStudentFromGroup);
        
            Assert.DoesNotContain(6, DbContext.Groups.First(x => x.Id == 3).Students.Select(x => x.Id));
            Assert.Null(DbContext.Users.OfType<Student>().First(x => x.Id == 6).GroupId);
        }
    }
}