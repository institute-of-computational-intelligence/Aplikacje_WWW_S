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
    [Authorize(Roles = "Teacher, Admin, Student")]
    public class GroupApiController : BaseApiController
    {
        private readonly IGroupService _groupService;
        private readonly ITeacherService _teacherService;
        private readonly UserManager<User> _userManager;
        public GroupApiController(ILogger logger, IMapper mapper,
        IGroupService groupService,
        ITeacherService teacherService,
        UserManager<User> userManager ) : base(logger, mapper)
        {
            _teacherService = teacherService;
            _groupService = groupService;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User?.Identity?.Name);
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                    return Ok(_groupService.GetGroups());
                else if (await _userManager.IsInRoleAsync(user, "Teacher"))
                {
                    if (user is Teacher teacher)
                        return Ok(_teacherService.GetTeacherGroups(Mapper.Map<TeacherVm>(user)));
                    return BadRequest("Teacher is assigned to role, but to the Teacher type.");
                }
                else if (await _userManager.IsInRoleAsync(user, "Student"))
                {
                    if (user is Student student)
                        return Ok(new []{_groupService.GetGroup(x => x.Id == student.GroupId)});
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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Post([FromBody] GroupVm groupVm)
        {
            return AddOrRemoveGroup(groupVm);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete([FromBody] GroupVm groupVm)
        {
            return AddOrRemoveGroup(groupVm);
        }

        private IActionResult AddOrRemoveGroup(GroupVm groupVm)
        {
            try
            {
                if (ModelState == null || !ModelState.IsValid)
                    return BadRequest(ModelState);
                _groupService.AddRemoveGroup(groupVm);
                return Ok(ModelState);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return BadRequest("Error occurred");
            }
        }
    }
}