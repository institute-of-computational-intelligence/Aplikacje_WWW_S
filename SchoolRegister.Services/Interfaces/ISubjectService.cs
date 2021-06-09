using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SchoolRegister.BLL.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface ISubjectService
    {
        SubjectVm AddOrUpdateSubject(AddOrUpdateSubjectVm addOrUpdateSubjectVm);
        SubjectVm GetSubject(Expression<Func<Subject, bool>> filterExpression);
        IEnumerable<SubjectVm> GetSubjects(Expression<Func<Subject, bool>> filterExpression = null);
    }
}