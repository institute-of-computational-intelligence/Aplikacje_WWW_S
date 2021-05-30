using System.ComponentModel.DataAnnotations;

public class RegisterNewUserVm
{
    [Required]
    [EmailAddress]
    [Display(Name = "Podaj E-mail")]
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "{0} - od {2} do {1} znaków.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "Hasła są niezgodne.")]
    public string ConfirmPassword { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "{0} - od {2} do {1} znaków.", MinimumLength = 3)]
    [DataType(DataType.Text)]
    [Display(Name = "First name")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "{0} - od {2} do {1} znaków.", MinimumLength = 4)]
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