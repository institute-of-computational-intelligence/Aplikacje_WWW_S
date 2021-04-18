using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
using System.Threading.Tasks;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGroupService
    {
        void AddGroup(GroupVm groupVm);
        void RemoveGroup(GroupVm groupVm);
        
    }
}