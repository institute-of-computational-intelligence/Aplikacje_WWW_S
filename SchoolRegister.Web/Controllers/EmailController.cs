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
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace SchoolRegister.Web.Controllers {
    public class EmailController : BaseController
    {
        private readonly IEmailSenderService _emailService;
        private readonly UserManager<User> _userManager;
        private readonly IParentService _parentService;

        public EmailController(ILogger logger, IMapper mapper, IStringLocalizer localizer, IEmailSenderService emailService, UserManager<User> userManager, IParentService parentService) : base(logger, mapper, localizer)
        {
            _emailService = emailService;
            _userManager = userManager;
            _parentService = parentService;
        }


        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public IActionResult SendEmail() {
            var user = _userManager.GetUserAsync(User).Result;
            if(_userManager.IsInRoleAsync(user, "Teacher").Result) {
                var parentsVm = _parentService.GetParents();

                ViewBag.ParentsSelectList = new SelectList(parentsVm.Select(p => new {
                    Text = $"{p.ParentName}",
                    Value = p.Id
                }), "Value", "Text");

                ViewBag.TeacherId = user.Id;

                return View();
            }
            else {
                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> SendEmail(EmailRequestVm emailModel, string parentName) {
            emailModel.ParentId = Convert.ToInt32(parentName);
            if (emailModel != null) { 
                await _emailService.SendEmailTeacher(emailModel);
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