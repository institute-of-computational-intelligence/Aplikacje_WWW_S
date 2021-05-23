using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Web.Controllers
{
    public class EmailController : BaseController
    {
        private readonly ITeacherService _teacherService;
        private readonly UserManager<User> _userManager;

        public EmailController(ILogger logger, IMapper mapper, IStringLocalizer localizer,
            UserManager<User> userManager, ITeacherService teacherService) : base(logger, mapper, localizer)
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
                await _teacherService.SendEmailAsync(emailVm);
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