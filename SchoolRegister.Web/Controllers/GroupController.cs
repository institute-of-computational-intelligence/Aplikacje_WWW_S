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

namespace SchoolRegister.Web.Controllers
{
    [Authorize(Roles = "Teacher, Admin, Student")]
    public class GroupController : BaseController
    {
        private readonly IGroupService _groupService;  
        private readonly ITeacherService _teacherService;  
        private readonly UserManager<User> _userManager;
        public GroupController(IGroupService groupService, ITeacherService teacherService, UserManager<User> userManager, IStringLocalizer localizer, ILogger logger, IMapper mapper) : base(logger, mapper, localizer)
        {
            _teacherService = teacherService; 
            _groupService = groupService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (_userManager.IsInRoleAsync(user, "Admin").Result)
                return View(_groupService.GetGroups());
            else if (_userManager.IsInRoleAsync(user, "Teacher").Result)
            {
                var teacher = _userManager.GetUserAsync(User).Result as Teacher;
                return View(_teacherService.GetTeacherGroups(Mapper.Map<TeacherVm>(teacher)));
            }
            else if (_userManager.IsInRoleAsync(user, "Student").Result)
            {
                var student = _userManager.GetUserAsync(User).Result as Student;
                
                return View(new []{_groupService.GetGroup(x => x.Id == student.GroupId)});
            }
            else
                return View("Error");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddOrRemoveGroup(int? id = null)
        {
            if (id.HasValue)
            {
                var groupVm = _groupService.GetGroup(x => x.Id == id);
                ViewBag.ActionType = Localizer["Remove"];
                return View(Mapper.Map<GroupVm>(groupVm));
            }
            ViewBag.ActionType = Localizer["Add"];
             return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult AddOrRemoveGroup(GroupVm groupVm)
        {
            if (ModelState.IsValid)
            {
                _groupService.AddRemoveGroup(groupVm);
                return RedirectToAction("Index");
            }
            return View();
        }
    }

}

