using System;
using System.Collections.Generic;
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

namespace SchoolRegister.Web.Controllers
{
    [Authorize(Roles = "Teacher, Admin, Student")]
    public class GroupController : BaseController
    {
        private readonly IGroupService _groupService;
        private readonly ISubjectService _subjectService;
        private readonly UserManager<User> _userManager;
        private readonly IStudentService _studentService;
        public GroupController(ILogger logger, IMapper mapper, IStringLocalizer localizer, IGroupService groupService, ISubjectService subjectService, UserManager<User> userManager, IStudentService studentService) : base(logger, mapper, localizer)
        {
            _groupService = groupService;
            _subjectService = subjectService;
            _userManager = userManager;
            _studentService = studentService;
        }

        public IActionResult Index()
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (_userManager.IsInRoleAsync(user, "Admin").Result || _userManager.IsInRoleAsync(user, "Teacher").Result)
                return View(_groupService.GetGroups());
            else if (_userManager.IsInRoleAsync(user, "Student").Result)
                return RedirectToAction("Details", "Student", new { studentId = user.Id });
            else
                return View("Error");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddOrEditGroup (int? id = null) 
        {
            if (id.HasValue) 
            {
                var group = _groupService.GetGroups(x => x.Id == id.Value).FirstOrDefault();
                ViewBag.ActionType = Localizer["Edit"];
                var groupDto = Mapper.Map<AddUpdateGroupVm> (group);
                return View (groupDto);
            }
            ViewBag.ActionType = Localizer["Add"];
            return View ();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult AddOrEditGroup (AddUpdateGroupVm addUpdateGroupVm) {
            if (ModelState.IsValid) {
                _groupService.AddOrUpdateGroup (addUpdateGroupVm);
                return RedirectToAction ("Index");
            }
            return View ();
        }

        public async Task<IActionResult> Details(int groupId)
        {
            var groupVm = _groupService.GetGroups(x => x.Id == groupId).FirstOrDefault();
            return View(groupVm);

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int groupId, DeleteGroupVm groupVm)
        {
            groupVm.Id = groupId;
            await _groupService.DeleteGroupAsync(groupVm);
            return RedirectToAction("Index");
        }
    }
}