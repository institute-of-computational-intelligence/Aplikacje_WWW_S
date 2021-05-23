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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRegister.Web.Controllers
{
    [Authorize(Roles = "Teacher, Admin, Student, Parent")]
    public class GradeController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IGradeService _gradeService;
        private readonly UserManager<User> _userManager;
        private readonly ITeacherService _teacherService;
        private readonly ISubjectService _subjectService;

        public GradeController(ILogger logger, IMapper mapper, IStringLocalizer localizer, IStudentService studentService, IGradeService gradeService, UserManager<User> userManager, ITeacherService teacherService, ISubjectService subjectService) : base(logger, mapper, localizer)
        {
            _studentService = studentService;
            _gradeService = gradeService;
            _userManager = userManager;
            _teacherService = teacherService;
            _subjectService = subjectService;
        }

        [Authorize(Roles = "Teacher, Admin, Student, Parent")]
        public async Task<IActionResult> GradesReport()
        {
            var user = _userManager.GetUserAsync(User).Result;

            if (_userManager.IsInRoleAsync(user, "Teacher").Result)
            {
                var teacher = _teacherService.GetTeachers(t => t.Id == user.Id).FirstOrDefault();
                var students = _studentService.GetStudents();
                // ViewBag.getterName = user.Id;
                ViewBag.StudentsSelectList = new SelectList(students.Select(s => new
                {
                    Text = $"{s.FirstName} {s.LastName}",
                    Value = s.StudentId
                }), "Value", "Text");
                return await Task.FromResult(View());
            }
            else if (_userManager.IsInRoleAsync(user, "Parent").Result)
            {
                var students = _studentService.GetStudents(x => x.ParentId == user.Id);
                // ViewBag.getterName = user.Id;
                ViewBag.StudentsSelectList = new SelectList(students.Select(s => new
                {
                    Text = $"{s.FirstName} {s.LastName}",
                    Value = s.StudentId
                }), "Value", "Text");
                return await Task.FromResult(View());
            }
            else if (_userManager.IsInRoleAsync(user, "Student").Result)
            {

                return RedirectToAction("GetGradesReport");
            }

            return await Task.FromResult(View());
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> AddGradeToStudent(int studentId)
        {
            var student = await _studentService.GetStudent(s => s.Id == studentId);
            ViewBag.Student = $"{student.FirstName} {student.LastName}";
            var subjects = _subjectService.GetSubjects(s => s.SubjectGroups.Any(sg => sg.GroupId == student.GroupId));
            ViewBag.SubjectsSelectList = new SelectList(subjects.Select(s => new
            {
                Text = $"{s.Name}",
                Value = s.Id
            }), "Value", "Text");
            ViewBag.GradeSelectList = new SelectList(Enum.GetValues(typeof(GradeScale)), "Choose grade");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> AddGradeToStudent(AddGradeToStudentVm addGradeToStudentVm, string subj)
        {
            var user = _userManager.GetUserAsync(User).Result;
            addGradeToStudentVm.TeacherId = user.Id;
            addGradeToStudentVm.SubjectId = Convert.ToInt32(subj);
            await _teacherService.AddGradeToStudent(addGradeToStudentVm);
            return RedirectToAction("Index", "Student");
        }

    }
}