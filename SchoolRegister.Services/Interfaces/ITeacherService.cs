using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces
{
    public interface ITeacherService
    {
        public void AddGrade (AddOrUpdateGradeVm addGradeVm);
        public void SendEmail (MailVm mailVm);
    }
}
