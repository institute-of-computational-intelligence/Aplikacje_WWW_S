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


namespace SchoolRegister.Web.Controllers
{
    [Authorize(Roles = "Teacher, Admin, Student")]
    public class GroupController : BaseController
    {
        private readonly IGroupService _groupService;
        private readonly UserManager<User> _userManager;

        public GroupController(IGroupService groupService,
        UserManager<User> userManager,
        IStringLocalizer localizer,
        ILogger logger,
        IMapper mapper) : base(logger, mapper, localizer)
        {
            _groupService = groupService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var user = _userManager.GetUserAsync(User).Result;

            if (_userManager.IsInRoleAsync(user, "Student").Result)
            {
                var student = _userManager.GetUserAsync(User).Result as Student;
                var studentGroup = _groupService.GetGroup(g => g.Id == student.GroupId);
                return View(new[] { studentGroup });

            }
            else if (_userManager.IsInRoleAsync(user, "Teacher").Result
                 || _userManager.IsInRoleAsync(user, "Admin").Result
            )
            {
                return View(_groupService.GetGroups());
            }
            return View("Error");


        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddRemoveGroup(int? id = null)
        {
            if (id.HasValue)
            {
                var groupVm = _groupService.GetGroup(x => x.Id == id);
                ViewBag.ActionType = "Remove";
                return View(Mapper.Map<GroupVm>(groupVm));
            }
            ViewBag.ActionType = "Add";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult AddRemoveGroup(GroupVm addOrRemoveGroup)
        {
            if (ModelState.IsValid)
            {
                _groupService.AddOrRemoveGroup(addOrRemoveGroup);
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}