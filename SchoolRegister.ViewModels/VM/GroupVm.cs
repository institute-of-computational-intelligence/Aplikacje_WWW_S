using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VM
{
  public class GroupVm
  {
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
  }
}