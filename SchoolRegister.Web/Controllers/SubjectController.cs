using System;
using System.Linq;
using System.Linq.Expressions;
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
    public class SubjectController : BaseController
    {
        private readonly ISubjectService _subjectService;
        private readonly ITeacherService _teacherService;
        private readonly IGroupService _groupService;
        private readonly UserManager<User> _userManager;
        public SubjectController(ISubjectService subjectService,
        ITeacherService teacherService,
        IGroupService groupService,
        UserManager<User> userManager,
        IStringLocalizer localizer,
        ILogger logger,
        IMapper mapper) : base(logger, mapper, localizer)
        {
            _subjectService = subjectService;
            _teacherService = teacherService;
            _userManager = userManager;
            _groupService = groupService;
        }
        public IActionResult Index()
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (_userManager.IsInRoleAsync(user, "Admin").Result)
                return View(_subjectService.GetSubjects());
            else if (_userManager.IsInRoleAsync(user, "Teacher").Result)
            {
                var teacher = _userManager.GetUserAsync(User).Result as Teacher;
                return View(_subjectService.GetSubjects(x => x.TeacherId == teacher.Id));
            }
            else if (_userManager.IsInRoleAsync(user, "Student").Result)
                return RedirectToAction("Details", "Student", new { studentId = user.Id });
            else
                return View("Error");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddOrEditSubject(int? id = null)
        {
            var teachersVm = _teacherService.GetTeachers();
            ViewBag.TeachersSelectList = new SelectList(teachersVm.Select(t => new
            {
                Text = $"{t.FirstName} {t.LastName}",
                Value = t.Id
            }), "Value", "Text");
            if (id.HasValue)
            {
                var subjectVm = _subjectService.GetSubject(x => x.Id == id);
                ViewBag.ActionType = "Edit";
                return View(Mapper.Map<AddOrUpdateSubjectVm>(subjectVm));
            }
            ViewBag.ActionType = "Add";
            return View();
        }
        public IActionResult Details(int id)
        {
            var subjectVm = _subjectService.GetSubject(x => x.Id == id);
            return View(subjectVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult AddOrEditSubject(AddOrUpdateSubjectVm addOrUpdateSubjectVm)
        {
            if (ModelState.IsValid)
            {
                _subjectService.AddOrUpdateSubject(addOrUpdateSubjectVm);
                return RedirectToAction("Index");
            }
            return View();
        }
        
        [Authorize(Roles = "Admin")]
        public IActionResult AttachSubjectToGroup (int subjectId) 
        {
            return AttachDetachSubjectToGroupGetView (subjectId);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DetachSubjectToGroup (int subjectId) 
        {
            return AttachDetachSubjectToGroupGetView (subjectId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult AttachSubjectToGroup (AttachDetachSubjectGroupVm attachDetachSubjectGroupVm) 
        {
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