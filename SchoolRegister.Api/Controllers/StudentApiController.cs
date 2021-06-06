using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using SchoolRegister.Services.Services;

namespace SchoolRegister.Api.Controllers
{
    [Authorize(Roles = "Teacher, Admin")]
    public class StudentApiController : BaseApiController
    {
        private readonly IStudentService _studentService;
        private readonly IGroupService _groupService;
        private readonly UserManager<User> _userManager;

        private readonly ApplicationDbContext _context;
        private readonly ITeacherService _teacherService;
        public StudentApiController(ILogger logger, IMapper mapper, IStudentService studentService, IGroupService groupService, UserManager<User> userManager, ApplicationDbContext context, ITeacherService teacherService) : base(logger, mapper)
        {
            _studentService = studentService;
            _groupService = groupService;
            _userManager = userManager;
            _context = context;
            _teacherService = teacherService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User?.Identity?.Name);
                if (_userManager.IsInRoleAsync(user, "Admin").Result)
                    return Ok(_studentService.GetStudents());
                if (_userManager.IsInRoleAsync(user, "Student").Result)
                    return BadRequest("Student can't request Student list");
                if (_userManager.IsInRoleAsync(user, "Parent").Result)
                    return Ok(_studentService.GetStudents(s => s.ParentId == user.Id));
                if (_userManager.IsInRoleAsync(user, "Teacher").Result)
                    return Ok(_studentService.GetStudents()); 
                return BadRequest("Try to log in first");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return BadRequest("Error occurred");
            }
        }
        
        // [HttpGet("{id:int:min(1)}")]
        // public async Task<IActionResult> Get(int id)
        // {
        //     try
        //     {
        //         var user = await _userManager.FindByNameAsync(User?.Identity?.Name);
        //         if (await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "Teacher") || await _userManager.IsInRoleAsync(user, "Parent"))
        //             return Ok(Mapper.Map<StudentVm>(_studentService.GetStudentAsync(s => s.Id == id)));
        //         else
        //             return BadRequest("Error occurred");
        //     }
        //     catch (ArgumentNullException ane)
        //     {
        //         Logger.LogError(ane, ane.Message);
        //         return NotFound();
        //     }
        //     catch (Exception ex)
        //     {
        //         Logger.LogError(ex, ex.Message);
        //         return BadRequest("Error occurred");
        //     }
        // }

        [HttpPost]
        public IActionResult AddStudentToGroup([FromBody] AttachDetachStudentToGroupVm attachStudentToGroupVm)
        {
            if(ModelState == null || !ModelState.IsValid)
                return BadRequest("Error occured");
            var studentVm = _groupService.AttachStudentToGroup(attachStudentToGroupVm);
            return Ok(studentVm);
        }
        [HttpPatch]
        public IActionResult DetachStudentFromGroup([FromBody] AttachDetachStudentToGroupVm detachStudentToGroupVm)
        {
            if(ModelState == null || !ModelState.IsValid)
                return BadRequest("Error occured");
            var studentVm = _groupService.DetachStudentFromGroup(detachStudentToGroupVm);
            return Ok(studentVm);
        }
    }
}        