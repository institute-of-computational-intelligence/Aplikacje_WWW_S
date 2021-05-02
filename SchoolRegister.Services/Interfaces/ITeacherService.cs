using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{

    public interface ITeacherService
    {
        void AddGrade(AddGradeVm addGradeVm);
        void SendEmail(SendEmailVm sendEmailVm);
    }
}