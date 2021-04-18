using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface ITeacherService
    {
        void AddGradeToStudent(AddGradeToStudentVm addGradeToStudent);
        void SendMailToStudentParent(SendMailToStudentParentVm sendMailToStudentParent);
    }
}