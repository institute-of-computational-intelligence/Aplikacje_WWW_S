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
    [Authorize(Roles = "Teacher, Admin")]
    public class TeacherApiController : BaseApiController
    {
        private readonly IEmailSenderService _emailSenderService;
        private readonly IGradeService _gradeService;     
        private readonly ITeacherService _teacherService;

        public TeacherApiController(ILogger logger, IMapper mapper,
            IEmailSenderService emailSenderService,
            IGradeService gradeService,
            ITeacherService teacherService): base(logger, mapper) 
        {
            _emailSenderService = emailSenderService;
            _gradeService = gradeService;
            _teacherService = teacherService;
        }
           
                  
        [HttpPost("AddGrade")]
        public IActionResult AddGrade(AddGradeVm addGradetVm) {
            try {
                 if(ModelState == null || !ModelState.IsValid)
                    return BadRequest(ModelState);
                var gradeVm = _gradeService.AddGrade(addGradetVm);
                return Ok(gradeVm);
            } catch(Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return BadRequest("Error occured");
            }
        }                  


        [HttpPost("EmailSender")]
        [Authorize(Roles ="Teacher, Admin")]
        public async Task<IActionResult> SendEmail(SendEmailVm sendEmailVm)
        {
            if(ModelState == null || !ModelState.IsValid)
                return BadRequest("Error occured");
            var emailVm = await _teacherService.SendEmailToParentAsync(sendEmailVm);
            return Ok(emailVm);
        }
    }
}