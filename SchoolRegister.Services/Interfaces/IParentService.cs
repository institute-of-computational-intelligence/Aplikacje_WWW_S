using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface IParentService
    {
           
        Task<ParentVm> GetParentAsync(Expression<Func<Parent, bool>> filterExpressions);

        IEnumerable<ParentVm> GetParents(Expression<Func<Parent, bool>> filterExpressions = null);
    }
}