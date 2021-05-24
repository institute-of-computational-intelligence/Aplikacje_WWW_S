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
                return View(_groupService.ShowGroups());
            else if (_userManager.IsInRoleAsync(user, "Student").Result)
                return RedirectToAction("Details", "Student", new { studentId = user.Id });
            else
                return View("Error");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddOrEditGroup(AddGroupVm addGroupVm)
        {
            if (!(addGroupVm is null))
            {
                var groupVm = await _groupService.AddGroupAsync(addGroupVm);
                ViewBag.ActionType = "Edit";
                return View(Mapper.Map<AddGroupVm>(groupVm));
            }
            ViewBag.ActionType = "Add";
            return View();

        }

        public async Task<IActionResult> Details(int groupId)
        {
            var groupVm = _groupService.ShowGroups(x => x.Id == groupId).FirstOrDefault();
            return View(groupVm);

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetIdToDelete(int groupId)
        {
            ViewBag.Id = groupId;
            return PartialView();
        }

        [HttpPost, ActionName("GetIdToDelete")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(DeleteGroupVm groupVm)
        {
            await _groupService.DeleteGroupAsync(groupVm);
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DetachStudentFromGroup(int studentId)
        {
            ViewBag.Student = await _studentService.ShowStudentAsync(s => s.Id == studentId);
            return View();
        }
    }
}