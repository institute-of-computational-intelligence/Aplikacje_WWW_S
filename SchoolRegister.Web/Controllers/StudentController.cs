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

        public IActionResult Index()
        {
            var user = _userManager.GetUserAsync(User).Result;

            if (_userManager.IsInRoleAsync(user, "Student").Result)
            {
                var student = _userManager.GetUserAsync(User).Result as Student;
                return View(new[] { _studentService.GetStudent(s => s.Id == student.Id) });

            }

            else if (_userManager.IsInRoleAsync(user, "Parent").Result)
            {
                var parent = _userManager.GetUserAsync(User).Result as Parent;

                return View(_studentService.GetStudents(s => s.ParentId == parent.Id));
            }
            else if (_userManager.IsInRoleAsync(user, "Teacher").Result
                 || _userManager.IsInRoleAsync(user, "Admin").Result
            )
            {

                return View(_studentService.GetStudents());
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
                var gg = _teacherService.AddGradeToStudent(grade).Result;
                return RedirectToAction("Index");
            }
            return View();
        }



    }
}