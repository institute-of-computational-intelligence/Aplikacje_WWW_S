using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SchoolRegister.BLL.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace SchoolRegister.Web.Controllers
{

    public class EmailController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly ITeacherService _teacherService;

        public EmailController(ILogger logger, IMapper mapper, IStringLocalizer localizer, UserManager<User> userManager, ITeacherService teacherService) : base(logger, mapper, localizer)
        {
            _userManager = userManager;
            _teacherService = teacherService;
        }


        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> SendEmail(SendEmailVm emailVm, string parentName)
        {
            if (emailVm != null)
            {
                _teacherService.SendEmailAsync(emailVm);
                return RedirectToAction("Succesful");
            }
            return RedirectToAction("Error");
        }

        public IActionResult Succesful()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}