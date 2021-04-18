using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace SchoolRegister.Services.Interfaces
{
    public interface IStudentService
    {
        public void AddStudentToGroup(AddOrRemStudentGroupVm addStudentToGroup);
        public void RemoveStudentFromGroup(AddOrRemStudentGroupVm removeStudentFromGroup);
    }
}