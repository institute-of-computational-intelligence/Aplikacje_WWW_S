using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SchoolRegister.BLL.DataModels;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;

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
                filterExpression = s => s.FirstName.Contains(filterValue);
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
            {
                if (user is Student)
                {
                    var student = _userManager.GetUserAsync(User).Result as Student;

                    Expression<Func<Student, bool>> groupFilterExpression = s => s.GroupId == student.GroupId;
                    Expression finalFilterBody;

                    if (filterExpression != null)
                    {
                        var invokedFilterExpr = Expression.Invoke(filterExpression, groupFilterExpression.Parameters);
                        finalFilterBody = Expression.AndAlso(groupFilterExpression.Body, invokedFilterExpr);
                    }
                    else finalFilterBody = groupFilterExpression.Body;

                    var finalFilterExpression = Expression.Lambda<Func<Student, bool>>(finalFilterBody, groupFilterExpression.Parameters);
                    var studentVms = _studentService.GetStudents(finalFilterExpression);
                    if (isAjaxRequest)
                        return PartialView("_StudentsTableDataPartial", studentVms);
                    return View(studentVms);
                }
                throw new Exception("Student is assigned to role, but its not type Student");
            }
            if (_userManager.IsInRoleAsync(user, "Parent").Result)
                return View(_studentService.GetStudents(s => s.ParentId == user.Id));
            if (_userManager.IsInRoleAsync(user, "Teacher").Result)
                return RedirectToAction("Index", "Student");

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
    }
}