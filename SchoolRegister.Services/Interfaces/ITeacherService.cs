using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface ITeacherService
    {       
        Task<Grade> AddGradeToStudent(AddGradeToStudentVm addGradeToStudent);
        void SendMailToStudentParent(SendMailToStudentParentVm sendMailToStudentParent);
    }
}