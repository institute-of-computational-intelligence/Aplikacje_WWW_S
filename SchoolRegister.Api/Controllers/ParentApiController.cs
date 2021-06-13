using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using SchoolRegister.Services.Services;
using System.Collections.Generic;


namespace SchoolRegister.Api.Controllers
{
    [Authorize(Roles = "Admin, Parent, Student")]
    
    public class ParentApiController : BaseApiController
    {
        private readonly IStudentService _studentService;
        private readonly UserManager<User> _userManager;

        public ParentApiController(ILogger logger, IMapper mapper, 
            IStudentService studentService, UserManager<User> userManager) : base(logger, mapper)
        {
            _studentService = studentService;
            _userManager = userManager;
        }
        [HttpGet("{id}")]
        public async Task <ActionResult<IEnumerable<GradeVm>>> GetGrades(int id)
        {
           var user = await _userManager.FindByNameAsync(User?.Identity?.Name);
            if(await _userManager.IsInRoleAsync(user, "Admin") || 
               await _userManager.IsInRoleAsync(user, "Parent") || 
               await _userManager.IsInRoleAsync(user, "Student"))
                return Ok(_studentService.GetStudent(s => s.Id == id));
            else
                return BadRequest("Error occured");    
        }
    }
}