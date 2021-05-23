using System;
using System.Collections.Generic;
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

namespace SchoolRegister.Web.Controllers
{
    [Authorize(Roles = "Teacher, Admin, Parent, Student")]
    public class StudentController : BaseController
    {
        private readonly ISubjectService _subjectService;
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;
        private readonly IGradeService _gradeService;

        private readonly UserManager<User> _userManager;
        public StudentController(ISubjectService subjectService, ITeacherService teacherService, IGradeService gradeService, IStudentService studentService, UserManager<User> userManager, IStringLocalizer localizer, ILogger logger, IMapper mapper) : base(logger, mapper, localizer)
        {
            _studentService = studentService;
            _gradeService = gradeService;
            _subjectService = subjectService;
            _teacherService = teacherService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (_userManager.IsInRoleAsync(user, "Admin").Result)
                return View(_studentService.GetStudents());
            else if (_userManager.IsInRoleAsync(user, "Teacher").Result)
               return View(_studentService.GetStudents());
            else if (_userManager.IsInRoleAsync(user, "Parent").Result)
            {
                var parent = _userManager.GetUserAsync(User).Result as Parent;
                return View(_studentService.GetStudents(x => x.ParentId == parent.Id));
            }
            else if (_userManager.IsInRoleAsync(user, "Student").Result)
                return RedirectToAction("Details", new { id = user.Id });
            else
                return View("Error");
        }


        public IActionResult Details(int id)
        {
            StudentVm studentvm = _studentService.GetStudent(x => x.Id == id);
            ViewBag.FirstName = studentvm.FirstName;
            ViewBag.LastName = studentvm.LastName;
            ViewBag.ActionType = "marks";
            GradesVm gradesvm = new GradesVm { CallerId = _userManager.GetUserAsync(User).Result.Id , StudentId = id };
            return View(_gradeService.ShowGrades(gradesvm));
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public IActionResult AddGrade(int id)
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
        public IActionResult AddGrade(AddGradeToStudentVm grade)
        {
            if (ModelState.IsValid)
            {
                var gr = _teacherService.AddGradeToStudent(grade).Result;
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveFromAddToGroup(int id, string name = null)
        {
            if (!String.IsNullOrEmpty(name))
            {
                var studentVm = _studentService.GetStudent(x => x.Id == id);
                ViewBag.ActionType = "Remove";
                return View(Mapper.Map<StudentVm>(studentVm));
            }
            ViewBag.ActionType = "Add";
             return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveFromAddToGroup(StudentVm studentVm)
        {
            if (ModelState.IsValid)
            {
                _studentService.AddOrRemoveStudentGroup(studentVm);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}