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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolRegister.Web.Controllers
{
    [Authorize(Roles = "Teacher, Admin, Student, Parent")]
    public class StudentController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IGradeService _gradeService;
        private readonly UserManager<User> _userManager;
        private readonly ITeacherService _teacherService;
        public StudentController(IStudentService studentService,
        UserManager<User> userManager,
        ITeacherService teacherService,
        IGradeService gradeService,
        IStringLocalizer localizer,
        ILogger logger,
        IMapper mapper) : base(logger, mapper, localizer)
        {
            _studentService = studentService;
            _userManager = userManager;
            _teacherService = teacherService;
            _gradeService = gradeService;
        }

        public IActionResult Index(string filterValue = null)
        {
            Expression<Func<Student, bool>> filterExpression = null;
            if (!string.IsNullOrWhiteSpace(filterValue))
                filterExpression = s => s.FirstName.Contains(filterValue);

            bool isAjaxRequest = HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest";

            var user = _userManager.GetUserAsync(User).Result;

            if (_userManager.IsInRoleAsync(user, "Student").Result)
            {
                if (user is Student studetn)
                {
                    Expression<Func<Student, bool>> filterStudentExpression = s => s.GroupId == studetn.GroupId;
                    Expression finalFilterBody;

                    if (filterExpression != null)
                    {
                        var invokeFilterExpression = Expression.Invoke(filterExpression, filterStudentExpression.Parameters);
                        finalFilterBody = Expression.AndAlso(filterStudentExpression.Body, invokeFilterExpression);
                    }
                    else
                        finalFilterBody = filterStudentExpression.Body;
                    var finalFilterExpression = Expression.Lambda<Func<Student, bool>>(finalFilterBody, filterStudentExpression.Parameters);

                    var studentVms = _studentService.GetStudents(finalFilterExpression);
                    if (isAjaxRequest)
                        return PartialView("_StudentsTableDataPartial", studentVms);
                    return View(studentVms);
                }
                throw new Exception("Student is assigned to role, but to the Student Type");
            }
            else if (_userManager.IsInRoleAsync(user, "Parent").Result)
            {
                if (user is Parent parent)
                {
                    Expression<Func<Student, bool>> filterStudentExpression = s => s.ParentId == parent.Id;
                    Expression finalFilterBody;

                    if (filterExpression != null)
                    {
                        var invokeFilterExpression = Expression.Invoke(filterExpression, filterStudentExpression.Parameters);
                        finalFilterBody = Expression.AndAlso(filterStudentExpression.Body, invokeFilterExpression);
                    }
                    else
                        finalFilterBody = filterStudentExpression.Body;
                    var finalFilterExpression = Expression.Lambda<Func<Student, bool>>(finalFilterBody, filterStudentExpression.Parameters);

                    var studentVms = _studentService.GetStudents(finalFilterExpression);
                    if (isAjaxRequest)
                        return PartialView("_StudentsTableDataPartial", studentVms);
                    return View(studentVms);
                }
                throw new Exception("Parent is assigned to role, but to the Parent Type");
            }
            else if (_userManager.IsInRoleAsync(user, "Teacher").Result
                 || _userManager.IsInRoleAsync(user, "Admin").Result
            )
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
        public IActionResult AddToGroup(int id)
        {
            var studentVm = _studentService.GetStudent(x => x.Id == id);
            ViewBag.ActionType = "Add";

            return View(Mapper.Map<StudentVm>(studentVm));

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveFromGroup(int id)
        {
            var studentVm = _studentService.GetStudent(x => x.Id == id);
            ViewBag.ActionType = "Remove";

            return View(Mapper.Map<StudentVm>(studentVm));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult AddToGroup(StudentVm studentVm)
        {
            if (ModelState.IsValid)
            {
                _studentService.AddStudentToGroup(studentVm);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveFromGroup(StudentVm studentVm)
        {
            if (ModelState.IsValid)
            {
                _studentService.RemoveStudentFromGroup(studentVm);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Student, Parent")]
        public IActionResult ShowGrades(int id)
        {
            var user = _userManager.GetUserAsync(User).Result;
            GetGradesVm getGradesVm = new GetGradesVm();
            ViewBag.ActionType = "Display";


            if (_userManager.IsInRoleAsync(user, "Parent").Result)
            {
                var parent = _userManager.GetUserAsync(User).Result as Parent;
                getGradesVm.CallerId = parent.Id;
                getGradesVm.StudentId = id;

                var resPar = _gradeService.DisplayGrades(getGradesVm).Result;
                return View(resPar);
            }
            getGradesVm.CallerId = id;
            getGradesVm.StudentId = id;
            var resStud = _gradeService.DisplayGrades(getGradesVm).Result;
            return View(resStud);

        }


        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public IActionResult AddGradeToStudent(int id)
        {
            var teacher = _userManager.GetUserAsync(User).Result as Teacher;

            ViewBag.StudentTeacherList = new AddGradeToStudentVm()
            {
                TeacherId = teacher.Id,
                StudentId = id
            };

            ViewBag.ActionType = "Add";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public IActionResult AddGradeToStudent(AddGradeToStudentVm grade)
        {
            if (ModelState.IsValid)
            {
                _teacherService.AddGradeToStudent(grade);
                return RedirectToAction("Index");
            }
            return View();
        }



        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public IActionResult SendEmailToParent(int parentId)
        {
            var teacher = _userManager.GetUserAsync(User).Result as Teacher;
            ViewBag.EmailInfo = new SendEmailToParentVm()
            {
                TeacherId = teacher.Id,
                ParentId = parentId
            };

            ViewBag.ActionType = "Send";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public IActionResult SendEmailToParent(SendEmailToParentVm emailToParentVm)
        {
            if (ModelState.IsValid)
            {
                _teacherService.SendEmailToParent(emailToParentVm);
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}