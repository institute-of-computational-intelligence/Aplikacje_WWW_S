using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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


        public IActionResult Index()
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (_userManager.IsInRoleAsync(user, "Admin").Result)
                return View(_studentService.ShowStudents());

            if (_userManager.IsInRoleAsync(user, "Student").Result)
                return RedirectToAction("Details", "Student", new { studentId = user.Id });

            if (_userManager.IsInRoleAsync(user, "Parent").Result)
                return View(_studentService.ShowStudents(s => s.ParentId == user.Id));

            if (_userManager.IsInRoleAsync(user, "Teacher").Result)
                return View(_studentService.ShowStudents());

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
        public async Task<ActionResult> Delete(RemoveFromGroupVm removefromgroup)
        {
            await _studentService.RemoveFromGroupAsync(removefromgroup);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Details(int studentId)
        {
            var student = await _studentService.ShowStudentAsync(s => s.Id == studentId);
            return View(student);
        }
    }
}