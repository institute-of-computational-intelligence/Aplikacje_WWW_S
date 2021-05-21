using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Web.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "Admin")]
    public class RegisterNewUsersDataModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;

        [BindProperty]
        public RegisterNewUserVm Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public RegisterNewUsersDataModel(UserManager<User> userManager,
            ILogger logger,
            ApplicationDbContext dbContext,
            IMapper mapper)
        {
            _userManager = userManager;
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public IActionResult OnGet()
        {
            var parentRole = _dbContext.Roles.FirstOrDefault(x => x.RoleValue == RoleValue.Parent);
            ViewData["Roles"] = new SelectList(_dbContext.Roles.Select(t => new
            {
                Text = t.Name,
                Value = t.Id
            }), "Value", "Text", parentRole?.Id);
            ViewData["Groups"] = new SelectList(_dbContext.Groups.Select(t => new
            {
                Text = t.Name,
                Value = t.Id
            }), "Value", "Text");
            ViewData["Parents"] = new SelectList(_dbContext.Users.OfType<Parent>().Select(t => new
            {
                Text = $"{t.FirstName} {t.LastName}",
                Value = t.Id
            }), "Value", "Text");
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var tupleUserRole = CreateUserBasedOnRole(Input);
                var result = await _userManager.CreateAsync(tupleUserRole.Item1, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    result = await _userManager.AddToRoleAsync(tupleUserRole.Item1, tupleUserRole.Item2.Name);
                    if (result.Succeeded)
                    {
                        OnGet();
                        StatusMessage = "User created";
                        return RedirectToPage();
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            OnGet();
            return RedirectToPage();
        }
        private Tuple<User, Role> CreateUserBasedOnRole(RegisterNewUserVm inputModel)
        {
            var role = _dbContext.Roles.FirstOrDefault(r => r.Id == inputModel.RoleId);
            if (role == null)
                throw new InvalidOperationException("Role not exists.");
            switch (role.RoleValue)
            {
                case RoleValue.Student:
                    return new Tuple<User, Role>(_mapper.Map<Student>(inputModel), role);
                case RoleValue.Parent:
                    return new Tuple<User, Role>(_mapper.Map<Parent>(inputModel), role);
                case RoleValue.Teacher:
                    return new Tuple<User, Role>(_mapper.Map<Teacher>(inputModel), role);
                case RoleValue.Admin:
                case RoleValue.User:
                    return new Tuple<User, Role>(_mapper.Map<User>(inputModel), role);
                default:
                    return null;
            }
        }
    }
}