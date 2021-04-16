using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.InterFaces
{
    public interface IStudentService
    { 
         public void AddStudentToGroup(AddOrRmvStudentGroup addStudentToGroup);
         public void RemoveStudentFromGroup(AddOrRmvStudentGroup removeStudentFromGroup);
    }
}