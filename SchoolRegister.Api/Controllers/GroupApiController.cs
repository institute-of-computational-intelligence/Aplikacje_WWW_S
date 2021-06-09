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
    [Authorize(Roles = "Teacher, Admin")]
    public class GroupApiController : BaseApiController
    {
        private readonly IGroupService _groupService;
        private readonly IStudentService _studentService;
        private readonly UserManager<User> _userManager;

        public GroupApiController(ILogger logger, IMapper mapper,
            IStudentService studentService,
            IGroupService groupService,
            UserManager<User> userManager) : base(logger, mapper)
        {
            _studentService = studentService;
            _userManager = userManager;
            _groupService = groupService;
        }

        [HttpPost]
        [Route("AddGroup")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody] AddGroupVm addGroupVm)
        {
            try
            {
                if (ModelState == null || !ModelState.IsValid)
                    return BadRequest(ModelState);
                var resultVm = await _groupService.AddGroupAsync(addGroupVm);
                return Ok(resultVm);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return BadRequest("Error occurred");
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("RemoveGroup/{id:int:min(1)}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _groupService.DeleteGroupAsync(new RemoveGroupVm {Id = id});
                return Ok(new {success = result});
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

        [HttpPost]
        [Route("AddStudentToGroup")]
        public async Task<IActionResult> AddStudentToGroup([FromBody] AddStudentToGroupVm addStudentToGroupVm)
        {
            try
            {
                if (ModelState == null || !ModelState.IsValid)
                    return BadRequest(ModelState);
                var resultVm = await _studentService.AddStudentToGroupAsync(addStudentToGroupVm);
                return Ok(resultVm);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return BadRequest("Error occurred");
            }
        }

        [HttpPost]
        [Route("RemoveStudentFromGroup")]
        public async Task<IActionResult> RemoveStudentFromGroup(
            [FromBody] RemoveStudentFromGroupVm removeStudentFromGroupVm)
        {
            try
            {
                if (ModelState == null || !ModelState.IsValid)
                    return BadRequest(ModelState);
                var resultVm = await _studentService.RemoveStudentFromGroupAsync(removeStudentFromGroupVm);
                return Ok(resultVm);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return BadRequest("Error occurred");
            }
        }
    }
}