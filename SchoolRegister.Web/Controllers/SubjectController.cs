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
        public IActionResult Index(string filterValue = null)
        {
            Expression<Func<Subject, bool>> filterExpression = null;
            if (!string.IsNullOrWhiteSpace(filterValue))
                filterExpression = s => s.Name.Contains(filterValue);
            bool isAjaxRequest = HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest";
            var user = _userManager.GetUserAsync(User).Result;
            if (_userManager.IsInRoleAsync(user, "Admin").Result)
            {
                var subjectVms = _subjectService.GetSubjects(filterExpression);
                if (isAjaxRequest)
                    return PartialView("_SubjectsTableDataPartial", subjectVms);
                return View(subjectVms);
            }
            else if (_userManager.IsInRoleAsync(user, "Teacher").Result)
            {
                if (user is Teacher teacher)
                {
                    Expression<Func<Subject, bool>> filterTeacherExpression = s => s.TeacherId == teacher.Id;
                    Expression finalFilterBody;
                    if (filterExpression != null)
                    {
                        var invokedFilterExpr = Expression.Invoke(filterExpression, filterTeacherExpression.Parameters);
                        finalFilterBody = Expression.AndAlso(filterTeacherExpression.Body, invokedFilterExpr);
                    }
                    else
                        finalFilterBody = filterTeacherExpression.Body;
                    var finalFilterExpression = Expression.Lambda<Func<Subject, bool>>(finalFilterBody, filterTeacherExpression.Parameters);
                    var subjectVms = _subjectService.GetSubjects(finalFilterExpression);
                    if (isAjaxRequest)
                        return PartialView("_SubjectsTableDataPartial", subjectVms);
                    return View(subjectVms);
                }
                throw new Exception("Teacher is assigned to role, but to the Teacher type.");
            }
            else if (_userManager.IsInRoleAsync(user, "Student").Result)
                return RedirectToAction("Details", "Student", new { studentId = user.Id });
            else if (_userManager.IsInRoleAsync(user, "Parent").Result)
                return RedirectToAction("Index", "Student");
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
                ViewBag.ActionType = Localizer["Edit"];
                return View(Mapper.Map<AddOrUpdateSubjectVm>(subjectVm));
            }
            ViewBag.ActionType = Localizer["Add"];
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
                ViewBag.ActionType = Localizer["Detach"];
            } else if (ControllerContext.ActionDescriptor.ActionName.StartsWith ("Attach")) {
                ViewBag.ActionType = Localizer["Attach"];
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