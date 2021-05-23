using System;
using System.Linq;
using System.Linq.Expressions;
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

namespace SchoolRegister.Web.Controllers {
    [Authorize (Roles = "Teacher")]
    public class TeacherController : BaseController {
        private readonly IStudentService _studentService;
        private readonly ISubjectService _subjectService;
        private readonly IGradeService _gradeService;
        private readonly ITeacherService _teacherService;
        private readonly UserManager<User> _userManager;
        public TeacherController (ILogger logger,
            IMapper mapper,
            IStringLocalizer localizer,
            IStudentService studentService,
            ISubjectService subjectService,
            IGradeService gradeService,
            ITeacherService teacherService,
            UserManager<User> userManager) : base (logger, mapper, localizer) {
            _studentService = studentService;
            _subjectService = subjectService;
            _gradeService = gradeService;
            _teacherService = teacherService;
            _userManager = userManager;
        }

        public IActionResult AddGrade (int? studentId = null) {
            if (!(User.IsInRole ("Admin") || User.IsInRole ("Teacher")))
                return RedirectToPage ("/Account/Login", new { area = "Identity" });
            var teacher = _userManager.GetUserAsync (User).Result;
            Expression<Func<Student, bool>> studentsExpression = null;
            Expression<Func<Subject, bool>> subjectsExpression = null;
            if (User.IsInRole ("Teacher")) {
                studentsExpression = s => s.Group.SubjectGroups.Any (sg => sg.Subject.TeacherId == teacher.Id);
                subjectsExpression = t => t.TeacherId == teacher.Id;
            }
            var students = _studentService.GetStudents (studentsExpression);
            var subjects = _subjectService.GetSubjects (subjectsExpression);
            ViewBag.StudentsList = new SelectList (students.Select (t => new {
                Text = $"{t.FirstName} {t.LastName}",
                    Value = t.Id,
                    Selected = t.Id == studentId

            }), "Value", "Text");
            ViewBag.SubjectList = new SelectList (subjects.Select (s => new {
                Text = s.Name,
                    Value = s.Id
            }), "Value", "Text");
            ViewBag.GradeScale = new SelectList (Enum.GetValues (typeof (GradeScale)).Cast<GradeScale> ().Select (x => new {
                Text = x.ToString (),
                    Value = ((int) x).ToString ()
            }), "Value", "Text");
            return View ();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddGrade (AddGradeToStudentVm addGradeToStudentVm) {
            try {
                if (ModelState.IsValid) {
                    var teacher = _userManager.GetUserAsync (User).Result;
                    addGradeToStudentVm.TeacherId = teacher.Id;
                    _gradeService.AddGradeToStudent (addGradeToStudentVm);
                    return RedirectToAction ("Details", "Student", new { studentId = addGradeToStudentVm.StudentId });
                }
            } catch (Exception ex) {
                Logger.LogError (ex, ex.Message);
                ModelState.AddModelError (string.Empty, ex.Message);
            }
            return AddGrade ();
        }

        public IActionResult SendEmailToParent (int studentId) {
            return View (new SendEmailToParentVm () { StudentId = studentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmailToParent (SendEmailToParentVm sendEmailToParentDto) {
            var teacher = _userManager.GetUserAsync (User).Result;
            sendEmailToParentDto.SenderId = teacher.Id;
            if (await _teacherService.SendEmailToParentAsync (sendEmailToParentDto)) {
                return RedirectToAction ("Index", "Student");
            }
            return View ("Error");
        }
    }
}