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
using SchoolRegister.Services.Services;

namespace SchoolRegister.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StudentApiController : BaseApiController
    {
        private readonly IGroupService _groupService;
        public StudentApiController(ILogger logger, IMapper mapper, IGroupService groupService) : base(logger, mapper)
        {
            _groupService = groupService;
        }

        [HttpPost]
        public IActionResult AddStudentToGroup([FromBody] AttachStudentGroupVm attachStudentToGroupVm)
        {
            if (ModelState == null || !ModelState.IsValid)
                return BadRequest("Error occured");
            var studentVm = _groupService.AttachStudentToGroup(attachStudentToGroupVm);
            return Ok(studentVm);
        }

        [HttpPatch]
        public IActionResult DetachStudentFromGroup([FromBody] AttachStudentGroupVm detachStudentToGroupVm)
        {
            if (ModelState == null || !ModelState.IsValid)
                return BadRequest("Error occured");
            var studentVm = _groupService.DetachStudentFromGroup(detachStudentToGroupVm);
            return Ok(studentVm);
        }
    }
}