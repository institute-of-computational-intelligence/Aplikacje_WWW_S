using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface ITeacherService
    {
        Task<bool> SendEmailToParent(SendEmailVm sendEmailVm);
        void AddGradeToStudentAsync(AddGradeToStudentVm addGradeToStudenVm);
    }
}