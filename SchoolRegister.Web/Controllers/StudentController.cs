using System;
using System.Collections.Generic;
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
using System.Threading.Tasks;

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

        public IActionResult Index(string filterValue = null)
        {
            Expression<Func<Student, bool>> filterExpression = null;
            if(!string.IsNullOrEmpty(filterValue))
                filterExpression = s => (s.FirstName+" "+s.LastName).Contains(filterValue);
            bool isAjaxRequest = HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest";

            var user = _userManager.GetUserAsync(User).Result;
            if (_userManager.IsInRoleAsync(user, "Admin").Result)
            {
                var studentVms = _studentService.GetStudents(filterExpression);
                if(isAjaxRequest)
                    return PartialView("_StudentsTableDataPartial", studentVms);
                return View(studentVms);
            }
            else if (_userManager.IsInRoleAsync(user, "Teacher").Result)
            {
                var studentVms = _studentService.GetStudents(filterExpression);
                if(isAjaxRequest)
                    return PartialView("_StudentsTableDataPartial", studentVms);
                return View(studentVms);
            }
            else if (_userManager.IsInRoleAsync(user, "Parent").Result)
            {
                if(user is Parent parent)
                {
                    Expression<Func<Student, bool>> filterParentExpression = s => s.ParentId == parent.Id;
                    Expression finalFilterBody;
                    if(filterExpression != null)
                    {
                        var invokedFilterExpr = Expression.Invoke(filterExpression, filterParentExpression.Parameters);
                        finalFilterBody = Expression.AndAlso(filterParentExpression.Body, invokedFilterExpr);
                    }
                    else
                        finalFilterBody = filterParentExpression.Body;
                    var finalFilterExpression = Expression.Lambda<Func<Student, bool>>(finalFilterBody, filterParentExpression.Parameters);
                    var studentVms = _studentService.GetStudents(finalFilterExpression);
                    if(isAjaxRequest)
                        return PartialView("_StudentsTableDataPartial", studentVms);
                    return View(studentVms);
                }
                throw new Exception("Parent is assigned to role, but to the Parent type.");
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
            ViewBag.ActionType = @Localizer["Grades"];
            GradesVm gradesvm = new GradesVm { CallerId = _userManager.GetUserAsync(User).Result.Id , StudentId = id };
            return View(_gradeService.ShowGrades(gradesvm));
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public IActionResult AddGrade(int id)
        {
            var teacher = _userManager.GetUserAsync(User).Result as Teacher;
            Expression<Func<Student, bool>> studentsExpression = s => s.Group.SubjectGroups.Any (sg => sg.Subject.TeacherId == teacher.Id);
            Expression<Func<Subject, bool>> subjectsExpression = t => t.TeacherId == teacher.Id;

            var students = _studentService.GetStudents (studentsExpression);
            var subjects = _subjectService.GetSubjects (subjectsExpression);

            var StudentTeacherList = new AddGradeToStudentVm()
            {
                TeacherId = teacher.Id,
                StudentId = id
            };
            
            ViewBag.ActionType = Localizer["Add"];

            return View(StudentTeacherList);
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
        public IActionResult RemoveFromAddToGroup(int id, string name=null)
        {
            if (!String.IsNullOrEmpty(name))
            {
                var studentVm = _studentService.GetStudent(x => x.Id == id);
                ViewBag.ActionType = Localizer["Remove"];
                return View(Mapper.Map<StudentVm>(studentVm));
            }
            ViewBag.ActionType = Localizer["Add"];
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