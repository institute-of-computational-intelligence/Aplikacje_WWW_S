using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface ITeacherService
    {
        public void AddGrade (AddGradeToStudentVm addGradeVm);
        public void SendEmail (MailVm mailVm);
    }
}
