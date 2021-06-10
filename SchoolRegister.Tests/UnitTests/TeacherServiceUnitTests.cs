using System;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using Xunit;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.Tests.UnitTests
{
       
    public class TeacherServiceUnitTests : BaseUnitTests
    {
        private readonly ITeacherService _teacherService;

        public TeacherServiceUnitTests(ApplicationDbContext dbContext, ITeacherService teacherService) : base(dbContext)
        {
            _teacherService = teacherService;  
        }

        [Fact]
        public async void AddGrade(){
            var addGrade = new AddGradeVm()
            {

                StudentId = 5,
                SubjectId = 5,
                GradeValue = GradeScale.DB,
                TeacherId = 1     
            
            };

            var grade = await _teacherService.AddGrade(addGrade);

          
                  
            Assert.Contains(grade, DbContext.Grades);
            Assert.NotNull(grade);
            Assert.Equal(5, grade.StudentId);
            Assert.Equal(5, grade.SubjectId);
            Assert.Equal(GradeScale.DB, grade.GradeValue);      


        }    

        [Fact]
        public async void AddGradeError()    
          
        {
            var addGradeToStudent = new AddGradeVm()
            {

                StudentId = 4,  
                SubjectId = 33,
                GradeValue = GradeScale.DB        
                     
            };

            await Assert.ThrowsAsync<ArgumentNullException>(() => _teacherService.AddGrade(addGradeToStudent));
        }

    }                 
}