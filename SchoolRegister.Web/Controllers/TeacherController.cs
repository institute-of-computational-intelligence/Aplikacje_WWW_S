using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Web.Controllers {
    [Authorize (Roles = "Teacher")]
    public class TeacherController : BaseController {
        private readonly IStudentService _studentService;
        private readonly ISubjectService _subjectService;
        private readonly IGradeService _gradeService;
        private readonly ITeacherService _teacherService;
        private readonly UserManager<User> _userManager;
        public TeacherController (ILogger logger,
            IMapper mapper,
            IStringLocalizer localizer,
            IStudentService studentService,
            ISubjectService subjectService,
            IGradeService gradeService,
            ITeacherService teacherService,
            UserManager<User> userManager) : base (logger, mapper, localizer) {
            _studentService = studentService;
            _subjectService = subjectService;
            _gradeService = gradeService;
            _teacherService = teacherService;
            _userManager = userManager;
        }

        public IActionResult SendEmailToParent (int studentId) 
        {
            return View (new SendEmailVm () { StudentId = studentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmailToParent (SendEmailVm sendEmailToParentDto) 
        {
            var teacher = _userManager.GetUserAsync (User).Result;
            sendEmailToParentDto.SenderId = teacher.Id;
            if (await _teacherService.SendEmailToParentAsync (sendEmailToParentDto)) 
            {
                return RedirectToAction ("Index", "Student");
            }
            return View ("Error");
        }
    }
}