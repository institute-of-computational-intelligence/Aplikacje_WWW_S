using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
namespace SchoolRegister.Model.DataModels
{
  public class Parent : User
  {
    public virtual IList<Student> Students { get; set; }

  }

}