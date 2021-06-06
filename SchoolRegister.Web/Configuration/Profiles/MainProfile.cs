using AutoMapper;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolRegister.Web.Configuration.Profiles
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            CreateMap<Subject, SubjectVm>()
                .ForMember(dest => dest.TeacherName, x => x.MapFrom(src => $"{src.Teacher.FirstName} {src.Teacher.LastName}"))
                .ForMember(dest => dest.Groups, x => x.MapFrom(src => src.SubjectGroups.Select(y => y.Group)));

            CreateMap<AddOrUpdateSubjectVm, Subject>();
            CreateMap<SubjectVm, AddOrUpdateSubjectVm>();

            CreateMap<Teacher, TeacherVm> ();
            CreateMap<Student, StudentVm> ();
            //GRADES
            CreateMap<Grade, GradeVm> ()
                .ForMember(dest => dest.Subject, y => y.MapFrom(src => src.Subject.Name))
                .ForMember(dest => dest.Student, y => y.MapFrom(src => string.Format("{0} {1}", src.Student.FirstName, src.Student.LastName)));
            //GROUP
            CreateMap<Group, GroupVm>();
            CreateMap<AddUpdateGroupVm, Group> ();
            CreateMap<GroupVm, AddUpdateGroupVm> ();
            //REGISTER
            CreateMap<RegisterNewUserVm, User>()
                .ForMember(dest => dest.UserName, y => y.MapFrom(src => src.Email))
                .ForMember(dest => dest.RegistrationDate, y => y.MapFrom(src => DateTime.Now));
            CreateMap<RegisterNewUserVm, Parent>()
                .ForMember(dest => dest.UserName, y => y.MapFrom(src => src.Email))
                .ForMember(dest => dest.RegistrationDate, y => y.MapFrom(src => DateTime.Now));
            CreateMap<RegisterNewUserVm, Student>()
                .ForMember(dest => dest.UserName, y => y.MapFrom(src => src.Email))
                .ForMember(dest => dest.RegistrationDate, y => y.MapFrom(src => DateTime.Now));
            CreateMap<RegisterNewUserVm, Teacher>()
                .ForMember(dest => dest.UserName, y => y.MapFrom(src => src.Email))
                .ForMember(dest => dest.RegistrationDate, y => y.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Title, y => y.MapFrom(src => src.TeacherTitles));
        }
    }
}
