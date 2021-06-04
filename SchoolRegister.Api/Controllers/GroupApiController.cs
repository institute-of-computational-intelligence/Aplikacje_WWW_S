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
namespace SchoolRegister.Api.Controllers
{
    [Authorize(Roles = "Teacher, Admin")]
    public class GroupApiController : BaseApiController
    {
        private readonly IGroupService _groupService;
        private readonly UserManager<User> _userManager;

        public GroupApiController(IGroupService groupService,
        UserManager<User> userManager,
        ILogger logger,
        IMapper mapper) : base(logger, mapper)
        {
            _groupService = groupService;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Teacher")]
        public IActionResult Get()
        {
            try
            {
                return Ok(_groupService.GetGroups());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return BadRequest("Error occurred");
            }
        }

        [HttpGet("{id:int:min(1)}")]
        [Authorize(Roles = "Admin, Teacher")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(_groupService.GetGroup(x => x.Id == id));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return BadRequest("Error occurred");
            }
        }




        [HttpPut]
        [Authorize(Roles = "Admin")]
        public IActionResult AddGroup([FromBody] GroupVm groupVm)
        {
            return AddOrRemoveGroup(groupVm);
        }

        [HttpDelete("{id:int:min(1)}")]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveGroup(int id)
        {
            var group = _groupService.GetGroup(g => g.Id == id);
            if (group is not null)
                return AddOrRemoveGroup(Mapper.Map<GroupVm>(group));
            return BadRequest("Error Occured");
        }

        private IActionResult AddOrRemoveGroup(GroupVm groupVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _groupService.AddOrRemoveGroup(groupVm);
                    return Ok(new { status = "Succes" });
                }
                return BadRequest(ModelState);
            }
            catch (Exception err)
            {
                Logger.LogError(err, err.Message);
                return BadRequest("Error occured");
            }
        }





    }
}
