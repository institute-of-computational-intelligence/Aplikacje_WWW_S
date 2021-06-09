using System;
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
    [Authorize(Roles = "Teacher")]
    public class EmailApiController : BaseApiController
    {
        private readonly ITeacherService _teacherService;
        private readonly UserManager<User> _userManager;

        public EmailApiController(ILogger logger, IMapper mapper,
            ITeacherService teacherService,
            UserManager<User> userManager) : base(logger, mapper)
        {
            _teacherService = teacherService;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("SendMailToParent")]
        public IActionResult SendEmailToParent([FromBody] SendEmailVm sendEmailVm)
        {
            try
            {
                if (ModelState == null || !ModelState.IsValid)
                    return BadRequest(ModelState);

                _teacherService.SendEmailAsync(sendEmailVm);
                return Ok(true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return BadRequest("Error occurred");
            }
        }
    }
}