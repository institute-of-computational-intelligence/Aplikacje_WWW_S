using Microsoft.AspNetCore.Identity;
namespace SchoolRegister.Model.DataModels
{
    public class Parent : User
    {
        public IUserLoginStore<Student> Students { get; set; }
    }
}