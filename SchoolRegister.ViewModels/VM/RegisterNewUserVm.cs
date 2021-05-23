using System.ComponentModel.DataAnnotations;

public class RegisterNewUserVm
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Text)]
    [Display(Name = "First name")]
    public string FirstName { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Text)]
    [Display(Name = "Last name")]
    public string LastName { get; set; }
    [Required]
    [Display(Name = "Role")]
    public int RoleId { get; set; }
    [Display(Name = "Parent")]
    public int? ParentId { get; set; }
    [Display(Name = "Teacher titles")]
    public string TeacherTitles { get; set; }
    public int? GroupId { get; set; }
}