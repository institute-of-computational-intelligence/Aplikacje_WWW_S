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
    [Authorize(Roles = "Teacher, Admin, Parent, Student")]
    public class StudentApiController : BaseApiController
    {
        private readonly IStudentService _studentService;
        private readonly IGradeService _gradeService;
        private readonly ITeacherService _teacherService;

        private readonly UserManager<User> _userManager;
        public StudentApiController(ILogger logger, IMapper mapper,
        IStudentService studentService,
        IGradeService gradeService,
        ITeacherService teacherService,
        UserManager<User> userManager ) : base(logger, mapper)
        {
            _gradeService = gradeService;
            _studentService = studentService;
            _teacherService = teacherService;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User?.Identity?.Name);
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                    return Ok(_studentService.GetStudents());
                else if (await _userManager.IsInRoleAsync(user, "Teacher"))
                {
                    if (user is Teacher teacher)
                        return Ok(_studentService.GetStudents());
                    return BadRequest("Teacher is assigned to role, but to the Teacher type.");
                }
                else if (await _userManager.IsInRoleAsync(user, "Parent"))
                {
                    if (user is Parent parent)
                        return Ok(_studentService.GetStudents(x => x.ParentId == user.Id));
                    return BadRequest("Parent is assigned to role, but to the Parent type.");
                }
                else if (await _userManager.IsInRoleAsync(user, "Student"))
                {
                    if (user is Student student)
                    {
                        var std = _studentService.GetStudent(x => x.Id == user.Id);
                        GradesVm gradesVm = new GradesVm{ CallerId = student.Id, StudentId = student.Id};
                        return Details(gradesVm);
                    }
                    return BadRequest("Student is assigned to role, but to the Student type.");
                }
                else
                    return BadRequest("Error occurred");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return BadRequest("Error occurred");
            }
        }

        [HttpGet("{id:int:min(1)}")]
        [Authorize(Roles="Parent")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User?.Identity?.Name);
                if (await _userManager.IsInRoleAsync(user, "Parent"))
                {
                    if (user is Parent parent)
                    {
                        GradesVm gradesVm = new GradesVm{ CallerId = user.Id, StudentId = id};
                        return Details(gradesVm);
                    }
                    return BadRequest("Parent is assigned to role, but to the Parent type.");
                }
                else
                    return BadRequest("Error occurred");
            }
            catch (ArgumentNullException ane)
            {
                Logger.LogError(ane, ane.Message);
                return NotFound();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return BadRequest("Error occurred");
            }
        }

        [HttpDelete("{id:int:min(1)}")]
        [Authorize(Roles="Admin")]
        public IActionResult Delete(int id)
        {
            var studentVm = _studentService.GetStudent(x => x.Id == id);
            return(AddRemoveToFromGroup(Mapper.Map<StudentVm>(studentVm)));
        }

        [HttpPut]
        [Authorize(Roles="Admin")]
        public IActionResult Put([FromBody] StudentVm studentVm)
        {
            return(AddRemoveToFromGroup(studentVm));
        }

        [HttpPatch]
        [Authorize(Roles="Teacher")]
        public async Task<IActionResult> Patch([FromForm] AddGradeToStudentVm addGradeVm)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User?.Identity?.Name);
                if (await _userManager.IsInRoleAsync(user, "Teacher"))
                {
                    if (user is Teacher teacher)
                    {
                        var grade = await _teacherService.AddGradeToStudent(addGradeVm);
                        return Ok(grade);
                    }
                    return BadRequest("Teacher is assigned to role, but to the Teacher type.");
                }
                else
                    return BadRequest("Error occurred");
            }
            catch (ArgumentNullException ane)
            {
                Logger.LogError(ane, ane.Message);
                return NotFound();
            }
        }

        private IActionResult AddRemoveToFromGroup(StudentVm studentVm)
        {
            try
            {
                if (ModelState == null || !ModelState.IsValid)
                    return BadRequest(ModelState);
                _studentService.AddOrRemoveStudentGroup(studentVm);
                return Ok(ModelState);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return BadRequest("Error occurred");
            }
        }

        private IActionResult Details(GradesVm gradesVm)
        {
            try
            {
                if (ModelState == null || !ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(_gradeService.ShowGrades(gradesVm));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return BadRequest("Error occurred");
            }
        }
    }
}