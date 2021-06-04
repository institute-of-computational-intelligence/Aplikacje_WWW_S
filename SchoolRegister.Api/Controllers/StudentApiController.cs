using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
namespace SchoolRegister.Api.Controllers
{
    [Authorize(Roles = "Teacher, Admin, Student, Parent")]
    public class StudentApiController : BaseApiController
    {
        private readonly IStudentService _studentService;
        private readonly IGradeService _gradeService;
        private readonly UserManager<User> _userManager;
        private readonly ITeacherService _teacherService;
        public StudentApiController(IStudentService studentService,
        UserManager<User> userManager,
        ITeacherService teacherService,
        IGradeService gradeService,
        ILogger logger,
        IMapper mapper) : base(logger, mapper)
        {
            _studentService = studentService;
            _userManager = userManager;
            _teacherService = teacherService;
            _gradeService = gradeService;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User?.Identity?.Name);
                if (await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "Admin"))
                    return Ok(_studentService.GetStudents());
                return BadRequest("Error occurred");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return BadRequest("Error occurred");
            }
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User?.Identity?.Name);
                if (await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "Teacher"))
                    return Ok(_studentService.GetStudent(x => x.Id == id));
                return BadRequest("Error occurred");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return BadRequest("Error occurred");
            }
        }



        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddToGroup([FromBody] StudentVm studentVm)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User?.Identity?.Name);
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    _studentService.AddStudentToGroup(studentVm);
                    return Ok(new { status = "Succes" });
                }
                return BadRequest("Acces denied");
            }
            catch (Exception err)
            {
                Logger.LogError(err, err.Message);
                return BadRequest("Error occured");
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveFromGroup([FromBody] StudentVm studentVm)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User?.Identity?.Name);

                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    _studentService.RemoveStudentFromGroup(studentVm);
                    return Ok(new { status = "Succes" });
                }
                return BadRequest("Acces denied");
            }
            catch (Exception err)
            {
                Logger.LogError(err, err.Message);
                return BadRequest("Error occured");
            }


        }

        [HttpGet("ShowGrades/{id:int:min(1)}")]
        [Authorize(Roles = "Student, Parent")]
        public async Task<IActionResult> ShowGrades(int id)
        {
            var user = await _userManager.FindByNameAsync(User?.Identity?.Name);
            GetGradesVm getGradesVm = new GetGradesVm();


            if (await _userManager.IsInRoleAsync(user, "Parent"))
            {
                var parent = _userManager.GetUserAsync(User).Result as Parent;
                getGradesVm.CallerId = parent.Id;
                getGradesVm.StudentId = id;

                return Ok(_gradeService.DisplayGrades(getGradesVm).Result);
            }
            else if (await _userManager.IsInRoleAsync(user, "Student"))
            {
                getGradesVm.CallerId = id;
                getGradesVm.StudentId = id;
                return Ok(_gradeService.DisplayGrades(getGradesVm).Result);

            }

            return BadRequest("AccesDenied");
        }

        [HttpPut]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> AddGrade([FromBody] AddGradeToStudentVm addGradeToStudentVm)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User?.Identity?.Name);
                if (await _userManager.IsInRoleAsync(user, "Teacher"))
                {
                    addGradeToStudentVm.TeacherId = user.Id;
                    var response = await _teacherService.AddGradeToStudent(addGradeToStudentVm);
                    return Ok(response);
                }
                return BadRequest("Acces denied");
            }
            catch (Exception err)
            {
                Logger.LogError(err, err.Message);
                return BadRequest("Error occured");
            }
        }
    }
}
