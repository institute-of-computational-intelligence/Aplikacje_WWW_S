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
    [Authorize(Roles = "Teacher,Student,Parent")]
    public class GradeApiController : BaseApiController
    {
        private readonly IGradeService _gradeService;
        private readonly ISubjectService _subjectService;
        private readonly ITeacherService _teacherService;
        private readonly UserManager<User> _userManager;

        public GradeApiController(ILogger logger, IMapper mapper,
            ITeacherService teacherService,
            UserManager<User> userManager,
            IGradeService gradeService) : base(logger, mapper)
        {
            _teacherService = teacherService;
            _userManager = userManager;
            _gradeService = gradeService;
        }

        [HttpPost]
        [Route("AddGrade")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> AddGrade([FromBody] AddGradeToStudentVm addGradeToStudentVm)
        {
            try
            {
                if (ModelState == null || !ModelState.IsValid)
                    return BadRequest(ModelState);
                var addGradeVm = await _teacherService.AddGradeToStudentAsync(addGradeToStudentVm);
                return Ok(addGradeVm);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return BadRequest("Error occurred");
            }
        }

        [HttpGet]
        [Route("GetGrades")]
        [Authorize(Roles = "Student,Parent")]
        public async Task<IActionResult> ViewGrade([FromQuery] GetGradesVm getGradesVm)
        {
            try
            {
                if (ModelState == null || !ModelState.IsValid)
                    return BadRequest(ModelState);
                var getGradesResult = await _gradeService.GetGradesAsync(getGradesVm);
                return Ok(getGradesResult);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return BadRequest("Error occurred");
            }
        }
    }
}