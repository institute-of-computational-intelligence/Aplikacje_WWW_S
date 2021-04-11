using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IStudentService
    {
        void AddToGroupAsync(AddToGroupVm addToGroupVm);
        void RemoveFromGroupAsync(RemoveFromGroupVm removeFromGroupVm);
    }
}