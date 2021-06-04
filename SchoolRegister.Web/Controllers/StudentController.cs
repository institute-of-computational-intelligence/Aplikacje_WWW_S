using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;

namespace SchoolRegister.Web.Controllers
{
    [Authorize(Roles = "Teacher, Admin, Student, Parent")]
    public class StudentController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IGroupService _groupService;
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;
        private readonly UserManager<User> _userManager;

        public StudentController(ILogger logger, IMapper mapper, IStringLocalizer localizer,
            IStudentService studentService, IGroupService groupService, UserManager<User> userManager,
            ApplicationDbContext context, ITeacherService teacherService) : base(logger, mapper, localizer)
        {
            _studentService = studentService;
            _groupService = groupService;
            _userManager = userManager;
            _context = context;
            _teacherService = teacherService;
        }


        public IActionResult Index()
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (_userManager.IsInRoleAsync(user, "Admin").Result)
                return View(_studentService.GetStudents());

            if (_userManager.IsInRoleAsync(user, "Student").Result)
                return RedirectToAction("Details", "Student", new { studentId = user.Id });

            if (_userManager.IsInRoleAsync(user, "Parent").Result)
                return View(_studentService.GetStudents(s => s.ParentId == user.Id));

            if (_userManager.IsInRoleAsync(user, "Teacher").Result)
                return View(_studentService.GetStudents());

            return View("Error");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetIdToDelete(int studentId)
        {
            ViewBag.Id = studentId;
            return PartialView();
        }

        [HttpPost]
        [ActionName("GetIdToDelete")]
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