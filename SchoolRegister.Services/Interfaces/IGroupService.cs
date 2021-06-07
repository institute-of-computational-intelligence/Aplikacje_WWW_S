using SchoolRegister.ViewModels.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGroupService
    {
        void DeleteGroupAsync(DeleteGroupVm deleteGroupVm);
        void AddGroupAsync(AddGroupVm addGroupVm);
    }
}
