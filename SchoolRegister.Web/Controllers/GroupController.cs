using System;
using System.Linq;
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

    [Authorize (Roles = "Teacher, Admin")]
    public class GroupController : BaseController {
        private readonly IGroupService _groupService;
        private readonly ISubjectService _subjectService;
        private readonly UserManager<User> _userManager;
        private readonly ITeacherService _teacherService;
        public GroupController (ILogger logger,
            IMapper mapper,
            IStringLocalizer localizer,
            IGroupService groupService,
            ISubjectService subjectService,
            UserManager<User> userManager,
            ITeacherService teacherService) : base (logger, mapper, localizer) {
            _groupService = groupService;
            _subjectService = subjectService;
            _userManager = userManager;
            _teacherService = teacherService;
        }
        public IActionResult Index () {
            var user = _userManager.GetUserAsync (User).Result;
            if (!_userManager.IsInRoleAsync (user, "Teacher").Result)
                return _userManager.IsInRoleAsync (user, "Admin").Result ?
                    View (_groupService.GetGroups ()) :
                    View ("Error");
            var getTeacherGroupsVm = new TeachersGroupsVm () {
                TeacherId = user.Id
            };
            return View (_teacherService.GetTeachersGroups (getTeacherGroupsVm));
        }

        public IActionResult Details (int id) {
            var group = _groupService.GetGroup (g => g.Id == id);
            return View (group);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AddOrEditGroup (int? id = null) {
            if (id.HasValue) {
                var group = _groupService.GetGroup (g => g.Id == id.Value);
                ViewBag.ActionType = "Edit";
                var groupDto = Mapper.Map<AddOrUpdateGroupVm> (group);
                return View (groupDto);
            }
            ViewBag.ActionType = "Add";
            return View ();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult AddOrEditGroup (AddOrUpdateGroupVm addOrUpdateGroupVm) {
            if (ModelState.IsValid) {
                _groupService.AddOrUpdateGroup (addOrUpdateGroupVm);
                return RedirectToAction ("Index");
            }
            return View ();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AttachSubjectToGroup (int subjectId) {
            return AttachDetachSubjectToGroupGetView (subjectId);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DetachSubjectToGroup (int subjectId) {
            return AttachDetachSubjectToGroupGetView (subjectId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult AttachSubjectToGroup (AttachDetachSubjectGroupVm attachDetachSubjectGroupVm) {
            try {
                if (!ModelState.IsValid) {
                    return View("AttachDetachSubjectToGroup");
                }
                _groupService.AttachSubjectToGroup (attachDetachSubjectGroupVm);
                return RedirectToAction ("Index", "Subject");
            } catch (Exception ex) {
                Logger.LogError (ex, ex.Message);
                ModelState.AddModelError (string.Empty, ex.Message);
                return AttachDetachSubjectToGroupGetView ();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DetachSubjectToGroup (AttachDetachSubjectGroupVm attachDetachSubjectGroupVm) {
            try {
                if (!ModelState.IsValid) {
                    return View("AttachDetachSubjectToGroup");
                }
                _groupService.DetachSubjectFromGroup (attachDetachSubjectGroupVm);
                return RedirectToAction ("Index", "Subject");
            } catch (Exception ex) {
                Logger.LogError (ex, ex.Message);
                ModelState.AddModelError (string.Empty, ex.Message);
                return AttachDetachSubjectToGroupGetView ();
            }
        }
        private IActionResult AttachDetachSubjectToGroupGetView (int? subjectId = null) {
            ViewBag.PostAction = ControllerContext.ActionDescriptor.ActionName;
            if (ControllerContext.ActionDescriptor.ActionName.StartsWith ("Detach")) {
                ViewBag.ActionType = "Detach";
            } else if (ControllerContext.ActionDescriptor.ActionName.StartsWith ("Attach")) {
                ViewBag.ActionType = "Attach";
            } else {
                return View ("Error");
            }
            var subjects = _subjectService.GetSubjects ()
                            .ToList();
            var groups = _groupService.GetGroups ();
            var currentSubject = subjects.FirstOrDefault (x => x.Id == subjectId);
            ViewBag.SubjectList = new SelectList (subjects.Select (s => new {
                Text = s.Name,
                    Value = s.Id,
                    Selected = s.Id == currentSubject?.Id
            }), "Value", "Text");
            ViewBag.GroupList = new SelectList (groups.Select (s => new {
                Text = s.Name,
                    Value = s.Id
            }), "Value", "Text");
            return View ("AttachDetachSubjectToGroup");
        }
    }
}