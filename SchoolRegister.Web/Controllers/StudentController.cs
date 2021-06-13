using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SchoolRegister.Model.DataModels;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Web.Controllers
{
    [Authorize(Roles = "Teacher, Admin, Student, Parent")]
    public class StudentController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IGroupService _groupService;
        private readonly UserManager<User> _userManager;

        private readonly ApplicationDbContext _context;
        private readonly ITeacherService _teacherService;

        public StudentController(ILogger logger, IMapper mapper, IStringLocalizer localizer, IStudentService studentService, IGroupService groupService, UserManager<User> userManager, ApplicationDbContext context, ITeacherService teacherService) : base(logger, mapper, localizer)
        {
            _studentService = studentService;
            _groupService = groupService;
            _userManager = userManager;
            _context = context;
            _teacherService = teacherService;
        }


        public IActionResult Index(string filterValue = null)
        {
            Expression<Func<Student, bool>> filterExpression = null;
            if (!string.IsNullOrWhiteSpace(filterValue))
                filterExpression = s => s.LastName.Contains(filterValue);
            bool isAjaxRequest = HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest";
            var user = _userManager.GetUserAsync(User).Result;
            if (_userManager.IsInRoleAsync(user, "Admin").Result)
            {
                var studentVms = _studentService.GetStudents(filterExpression);
                if (isAjaxRequest)
                    return PartialView("_StudentsTableDataPartial", studentVms);
                return View(studentVms);
            }
            if (_userManager.IsInRoleAsync(user, "Student").Result)
                return RedirectToAction("Details", "Student", new { studentId = user.Id });

            if (_userManager.IsInRoleAsync(user, "Parent").Result)
                return View(_studentService.GetStudents(s => s.ParentId == user.Id));

            if (_userManager.IsInRoleAsync(user, "Teacher").Result)
            {
                    var studentVms = _studentService.GetStudents(filterExpression);
                if (isAjaxRequest)
                    return PartialView("_StudentsTableDataPartial", studentVms);
                return View(studentVms);
            }
            return View("Error");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetIdToDelete(int studentId)
        {
            ViewBag.Id = studentId;
            return PartialView();
        }

        [HttpPost, ActionName("GetIdToDelete")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int studentId)
        {
            await _studentService.RemoveStudentAsync(studentId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Details(int studentId)
        {
            var student = await _studentService.GetStudentAsync(s => s.Id == studentId);
            return View(student);
        }

        [Authorize(Roles = "Teacher, Admin")]
        public IActionResult AttachStudentToGroup(int studentId)
        {
            ViewBag.ActionType = Localizer["Attach"];
            return AttachDetachGetView(studentId);
        }
        [Authorize(Roles = "Teacher, Admin")]
        public IActionResult DetachStudentToGroup(int studentId)
        {
            ViewBag.ActionType = Localizer["Detach"];
            return AttachDetachGetView(studentId);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher, Admin")]
        public IActionResult AttachStudentToGroup(AttachDetachStudentToGroupVm attachDetachStudentToGroupVm)
        {
            if (ModelState.IsValid)
            {
                _groupService.AttachStudentToGroup(attachDetachStudentToGroupVm);
                return RedirectToAction("Index");
            }
            return View("AttachDetachStudentToGroup");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher, Admin")]
        public IActionResult DetachStudentToGroup(AttachDetachStudentToGroupVm attachDetachStudentToGroupVm)
        {
            if (ModelState.IsValid)
            {
                _groupService.DetachStudentFromGroup(attachDetachStudentToGroupVm);
                return RedirectToAction("Index");
            }
            return View("AttachDetachStudentToGroup");
        }
        [Authorize(Roles = "Teacher, Admin")]
        private IActionResult AttachDetachGetView(int studentId)
        {
            var students = _studentService.GetStudents()
                                        .ToList();
            var groups = _groupService.GetGroups();
            var currentStudent = students.FirstOrDefault(x => x.Id == studentId);
            if (currentStudent == null)
            {
                throw new ArgumentNullException($"studentId not exists.");
            }
            var attachDetachStudentToGroupDto = new AttachDetachStudentToGroupVm
            {
                StudentId = currentStudent.Id
            };
            ViewBag.SubjectList = new SelectList(students.Select(s => new
            {
                Text = $"{s.FirstName} {s.LastName}",
                Value = s.Id,
                Selected = s.Id == currentStudent.Id
            }), "Value", "Text");
            ViewBag.GroupList = new SelectList(groups.Select(s => new
            {
                Text = s.Name,
                Value = s.Id
            }), "Value", "Text");
            return View("AttachDetachStudentToGroup", attachDetachStudentToGroupDto);
        }
    }
}