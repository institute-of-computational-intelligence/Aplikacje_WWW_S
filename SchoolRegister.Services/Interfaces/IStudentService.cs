using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
namespace SchoolRegister.Services.Interfaces
{
    public interface IStudentService
    {
         void AddStudentToGroup(StudentVm studentVm);
         void RemoveStudentFromGroup(StudentVm studentVm);
    }
}