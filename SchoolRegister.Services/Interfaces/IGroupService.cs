using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGroupService
    {
         void DeleteGroupAsync(DeleteGroupVm deleteGroupVm);
         void AddGroupAsync(AddGroupVm addGroupVm);
    }
}