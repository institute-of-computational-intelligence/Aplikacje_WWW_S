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

namespace SchoolRegister.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GroupApiController : BaseApiController
    {
        private readonly IGroupService _groupService;
        public GroupApiController(ILogger logger, IMapper mapper, 
                IGroupService groupService ) : base(logger, mapper)
        {
            _groupService = groupService;
        }

        [HttpPost]
        public IActionResult CreateGroup([FromBody] AddGroupVm addOrUpdateGroupVm)
        {
            try 
            {
                if(ModelState == null || !ModelState.IsValid)
                    return BadRequest("Error occurrrrred");
                var groupVm = _groupService.AddGroup(addOrUpdateGroupVm);
                return Ok(groupVm);
            } catch(Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return BadRequest("Error occured");
            }
        }
    }
}